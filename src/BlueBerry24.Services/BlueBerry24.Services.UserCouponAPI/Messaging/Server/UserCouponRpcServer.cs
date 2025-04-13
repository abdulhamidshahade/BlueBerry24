using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using BlueBerry24.Services.UserCouponAPI.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace BlueBerry24.Services.UserCouponAPI.Messaging.Server
{
    public class UserCouponRpcServer : BackgroundService
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "UserCouponServer.direct";
        private IConnection _connection;
        private IChannel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly Dictionary<string, Func<IUserCouponService, byte[], Task<byte[]>>> _handlers =
            new Dictionary<string, Func<IUserCouponService, byte[], Task<byte[]>>>();

        public UserCouponRpcServer(IConfiguration configuration,
                               IServiceScopeFactory scopeFactory)
        {
            _rabbitMqHost = configuration["RabbitMq:Host"];
            _scopeFactory = scopeFactory;

            _handlers.Add("CheckUserCoupon", HandleCheckUserCoupon);
            _handlers.Add("DisableUserCoupon", HandleDisableCoupon);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMqConnection();

            foreach (var rountingKey in _handlers.Keys)
            {
                var queueName = $"UserCoupon_{rountingKey}";

                await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false);
                await _channel.QueueBindAsync(queueName, exchange: _exchange, routingKey: rountingKey);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) => await ProcessMessage(ea, rountingKey);

                await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);
            }

        }

        private async Task ProcessMessage(BasicDeliverEventArgs ea, string routingKey)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IUserCouponService>();


                if(!_handlers.TryGetValue(routingKey, out var handler))
                {
                    throw new InvalidOperationException("Unknown routing key");
                }

                var responseBytes = await handler(service, ea.Body.ToArray());

                var properties = new BasicProperties
                {
                    CorrelationId = ea.BasicProperties.CorrelationId
                };

                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: ea.BasicProperties.ReplyTo,
                    basicProperties: properties,
                    body: responseBytes,
                    mandatory: true
                    );

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch(Exception ex)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        }

        private async Task<byte[]> HandleCheckUserCoupon(IUserCouponService service, byte[] body)
        {
            var request = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(body));


            bool result = await service.IsCouponUsedByUser(request.UserId, request.CouponId);
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { Result = result }));
        }

        private async Task<byte[]> HandleDisableCoupon(IUserCouponService service, byte[] body)
        {
            var request = JsonConvert.DeserializeObject<dynamic>(
                Encoding.UTF8.GetString(body));

            var result = await service.DisableUserCouponAsync(request.UserId, request.CouponCode);
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
        }


        public async Task InitializeRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "IsCouponUsedByUser", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "IsCouponUsedByUser", exchange: _exchange, routingKey: "CheckUserCoupon");
        }
    }
}
