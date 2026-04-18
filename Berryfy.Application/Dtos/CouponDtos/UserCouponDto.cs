namespace Berryfy.Application.Dtos.CouponDtos
{
    public class UserCouponDto
    {
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
