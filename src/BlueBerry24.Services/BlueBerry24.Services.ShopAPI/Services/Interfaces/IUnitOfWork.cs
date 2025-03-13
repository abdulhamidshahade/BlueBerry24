namespace BlueBerry24.Services.ShopAPI.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Dispose();
        Task<int> SaveChangesAsync();
    }
}
