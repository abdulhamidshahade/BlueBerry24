using BlueBerry24.Services.ShoppingCartAPI.Messaging.Client;
using BlueBerry24.Services.ShoppingCartAPI.Models.DTOs;
using BlueBerry24.Services.ShoppingCartAPI.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BlueBerry24.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly string _rabbitMqHost;
        private readonly string _exchange = "CheckCouponAvailability.direct";
        private IConnection _connection;
        private IChannel _channel;
        private readonly IConfiguration _config;
        private readonly ICartService _cartService;

        public CouponService(IConfiguration config, ICartService cartService)
        {
            _rabbitMqHost = config["RabbitMq:host"];
            InitializeRabbitMqConnection().Wait();
            _config = config;
            _cartService = cartService;
        }

        private async Task InitializeRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHost };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "CheckCouponByCode", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "CheckCouponByCode", exchange: _exchange, routingKey: "CheckCoupon");
        }

        //public async Task<bool> IsAvailableAsync(string couponCode)
        //{
          
        //    await _channel.ExchangeDeclareAsync(exchange: _exchange, type: "direct", durable: true, autoDelete: false, arguments: null);
        //    await _channel.QueueDeclareAsync(queue: "CheckCouponByCoupon", durable:true, exclusive: false, autoDelete: false, arguments: null);
        //    await _channel.QueueBindAsync(queue: "CheckCouponByCoupon", exchange: _exchange, routingKey: "CheckCoupon");

        //    string message = JsonConvert.SerializeObject(couponCode);
        //    var body = Encoding.UTF8.GetBytes(message);

        //    await _channel.BasicPublishAsync(exchange: _exchange, routingKey: "CheckCoupon", body: body);
        //    Console.WriteLine($"[ShoppingCartAPI] Sent: {message}");
        //}





        public Task<bool> IsUsedByUserAsync(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }


        public async Task<CouponDto> GetCouponByNameAsync(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> RedeemCouponAsync(string userId, string headerId, string couponCode, decimal total)
        {
            CouponRpcClient couponClient = new CouponRpcClient(_config);
            var couponAvailability = await couponClient.IsCouponAvaiableAsync(couponCode);

            UserRpcClient userClient = new UserRpcClient(_config);
            var userAvailability = await userClient.IsUserAvailableAsync(userId);

            var headerAvailability = await _cartService.ExistsByHeaderIdAsync(userId, headerId);

            if (couponAvailability == null || !userAvailability || !headerAvailability)
            {
                throw new InvalidOperationException("coupon, user or header are not available!");
            }

            UserCouponRpcClinet userCouponClient = new UserCouponRpcClinet(_config);
            var isUserHasCoupon = await userCouponClient.IsUserHasCouponAsync(couponCode, userId);

            if (!isUserHasCoupon)
            {
                throw new NotImplementedException($"The coupon is not belong to the user with user Id: {userId}");
            }


            var totalAppliedCoupon = await ApplyCoupon(total, couponAvailability);

            return totalAppliedCoupon;
        }


        private async Task<decimal> ApplyCoupon(decimal total, CouponDto coupon)
        {
            if (coupon.MinimumAmount <= total)
            {
                total = total - coupon.DiscountAmount;
                return total;
            }

            return total;
        }

        public async Task DisableCouponByUserId(string userId, string couponCode)
        {
            UserCouponClient userCouponClient = new UserCouponClient(_config);
            await userCouponClient.DisableCouponByUserId(userId, couponCode);
        }
    }
}
