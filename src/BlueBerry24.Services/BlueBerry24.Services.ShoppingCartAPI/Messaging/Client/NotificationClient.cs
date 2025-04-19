using BlueBerry24.Services.ShoppingCartAPI.Messaging.Client.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Runtime.InteropServices;
using System.Text;

namespace BlueBerry24.Services.ShoppingCartAPI.Messaging.Client
{
    public class NotificationClient : INotificationClient
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "NotificationService.direct";
        private IConnection _connection;
        private IChannel _channel;

        public NotificationClient(IConfiguration configuration)
        {
            _rabbitMqHost = configuration["RabbitMq:Host"];
        }

        public async Task NotifyShopByProductIncreament(string shopId, string productId, int count)
        {
            await InitializeRabbitMqConnection();

            await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true);
            string jsonMessage = JsonConvert.SerializeObject(new { ShopId = shopId, ProductId = productId, Count = count });

            byte[] body = Encoding.UTF8.GetBytes(jsonMessage);

            await _channel.BasicPublishAsync(exchange: _exchange, routingKey: "NotifiyShopIncreament", body: body);
        }


        private async Task InitializeRabbitMqConnection()
        {
            if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen) return;

            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }

        public void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            _channel?.Dispose();
            _connection?.CloseAsync();
        }
    }
}
