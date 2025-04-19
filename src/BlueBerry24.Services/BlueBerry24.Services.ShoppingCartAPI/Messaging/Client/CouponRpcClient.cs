using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BlueBerry24.Services.ShoppingCartAPI.Messaging.Client
{
    public class CouponRpcClient
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "CheckCouponAvailability.direct";
        private IConnection _connection;
        private IChannel _channel;

        public CouponRpcClient(IConfiguration configuration)
        {
            _rabbitMqHost = configuration["RabbitMq:Host"];
        }

        public async Task<CouponDto> IsCouponAvaiableAsync(string couponCode)
        {
            await InitializeRabbitMqConnection();
            var correlationId = Guid.NewGuid().ToString();
            var replyQueueName = await _channel.QueueDeclareAsync("", exclusive: true);
            var consumer = new AsyncEventingBasicConsumer(_channel);

            var tcs = new TaskCompletionSource<CouponDto>();

            consumer.ReceivedAsync += async (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var response = JsonConvert.DeserializeObject<CouponDto>(message);

                    tcs.SetResult(response);
                }
            };

            await _channel.BasicConsumeAsync(queue: replyQueueName.QueueName, autoAck: true, consumer: consumer);

            string message = JsonConvert.SerializeObject(couponCode);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                ReplyTo = replyQueueName.QueueName,
                CorrelationId = correlationId

            };

            await _channel.BasicPublishAsync(exchange: _exchange, routingKey: "CheckCoupon", basicProperties: properties, body: body, mandatory: true);

            var timeout = Task.Delay(TimeSpan.FromSeconds(10));
            var completedTask = await Task.WhenAny(tcs.Task, timeout);

            return completedTask == tcs.Task ? await tcs.Task : throw new TimeoutException("Coupon check time out");
        }



        private async Task InitializeRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }
    }
}