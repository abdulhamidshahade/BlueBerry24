namespace BlueBerry24.Services.UserCouponAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<bool> IsCouponExistsByIdAsync(string couponId);
    }
}
