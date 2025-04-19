using BlueBerry24.Services.ShoppingCartAPI.Messaging.Client.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BlueBerry24.Services.ShoppingCartAPI.Messaging.Client
{
    public class StockRpcClient : IStockRpcClient
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "StockService.direct";
        private IConnection _connection;
        private IChannel _channel;


        public StockRpcClient(IConfiguration configuration)
        {
            _rabbitMqHost = configuration["RabbitMq:Host"];
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsProductAvailableInStockAsync(string productId, string shopId)
        {
            await InitializeRabbitMqConnection();

            var correlationId = Guid.NewGuid().ToString();
            var replyQueue = await _channel.QueueDeclareAsync("", exclusive: true);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            var tcs = new TaskCompletionSource<bool>();

            consumer.ReceivedAsync += async (_, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var response = JsonConvert.DeserializeObject<dynamic>(message);

                    tcs.SetResult(response.IsAvailable);
                }
            };

            await _channel.BasicConsumeAsync(queue: replyQueue, autoAck: true, consumer: consumer);


            string message = JsonConvert.SerializeObject(new { ProductId = productId, ShopId = shopId });
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                ReplyTo = replyQueue.QueueName,
                CorrelationId = correlationId
            };

            await _channel.BasicPublishAsync(exchange: _exchange, routingKey: "IsProductAvailableInStock", basicProperties: properties, body: body, mandatory: true);

            TimeSpan timeoutDuration = TimeSpan.FromSeconds(10);
            var timeout = Task.Delay(timeoutDuration);
            
            var completedTask = await Task.WhenAny(tcs.Task, timeout);

            return completedTask == tcs.Task ? await tcs.Task : throw new TimeoutException("Producut check time out");
        }


        private async Task InitializeRabbitMqConnection()
        {
            if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen) return;
           
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }

    }
}
