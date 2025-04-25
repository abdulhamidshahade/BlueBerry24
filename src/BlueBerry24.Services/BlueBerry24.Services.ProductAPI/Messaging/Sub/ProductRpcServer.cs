
using BlueBerry24.Services.ProductAPI.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlueBerry24.Services.ProductAPI.Messaging.Sub
{
    public class ProductRpcServer : BackgroundService
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "ProductService.direct";
        private IConnection _connection;
        private IChannel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductRpcServer(IConfiguration configuration,
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
                using var scope = _scopeFactory.CreateScope();

                var couponService = scope.ServiceProvider.GetRequiredService<IProductService>();
            };
        }


        public async Task InitializeRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "CheckCouponByCode", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "CheckCouponByCode", exchange: _exchange, routingKey: "IsProductAvailableInStock");
        }
    }
}
