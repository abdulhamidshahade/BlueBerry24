using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.CouponAPI.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BlueBerry24.Services.CouponAPI.Messaging
{
    public class CouponRpcServer : BackgroundService
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "CheckCouponAvailability.direct";
        private IConnection _connection;
        private IChannel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public CouponRpcServer(IConfiguration configuration, 
                               IServiceScopeFactory scopeFactory)
        {
            _rabbitMqHost = configuration["RabbitMq:Host"];
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMqConnection();
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var couponService = scope.ServiceProvider.GetRequiredService<ICouponService>();
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var couponCode = JsonConvert.DeserializeObject<string>(message);

                    CouponDto coupon = await couponService.GetByCodeAsync(couponCode);

                    var properties = new BasicProperties
                    {
                        CorrelationId = ea.BasicProperties.CorrelationId
                    };

                    var response = JsonConvert.SerializeObject(coupon);

                    var responseBody = Encoding.UTF8.GetBytes(response);

                    await _channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: ea.BasicProperties.ReplyTo,
                        basicProperties: properties,
                        body: responseBody,
                        mandatory: true
                        );

                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);


                }
                catch(Exception ex)
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                }
                
            };

            await _channel.BasicConsumeAsync(queue: "CheckCouponByCode", autoAck: false, consumer: consumer);


        }



        public async Task InitializeRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "CheckCouponByCode", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "CheckCouponByCode", exchange: _exchange, routingKey: "CheckCoupon");
        }

        
    }
}

