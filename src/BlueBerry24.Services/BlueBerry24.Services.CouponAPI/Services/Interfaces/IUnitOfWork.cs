namespace BlueBerry24.Services.CouponAPI.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Dispose();
        Task SaveChangesAsync();
    }
}
