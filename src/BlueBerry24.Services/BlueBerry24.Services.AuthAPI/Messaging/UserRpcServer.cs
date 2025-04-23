
using BlueBerry24.Services.AuthAPI.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BlueBerry24.Services.CouponAPI.Messaging
{
    public class UserRpcServer : BackgroundService
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "CheckUserAvailability.direct";
        private IConnection _connection;
        private IChannel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public UserRpcServer(IConfiguration configuration, 
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
                using (var scope = _scopeFactory.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var userId = JsonConvert.DeserializeObject<string>(message);

                    bool isExists = await userService.IsUserExistsByIdAsync(userId);

                    var properties = new BasicProperties
                    {
                        CorrelationId = ea.BasicProperties.CorrelationId
                    };

                    var responseBody = Encoding.UTF8.GetBytes(isExists.ToString());

                    await _channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: ea.BasicProperties.ReplyTo,
                        basicProperties: properties,
                        body: responseBody,
                        mandatory: true
                        );
                }
            };

            await _channel.BasicConsumeAsync(queue: "CheckUserById", autoAck: false, consumer: consumer);


        }



        public async Task InitializeRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "CheckUserById", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "CheckUserById", exchange: _exchange, routingKey: "CheckUser");
        }

        
    }
}

