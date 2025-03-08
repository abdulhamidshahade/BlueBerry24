namespace BlueBerry24.Services.ProductAPI.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void Dispose();
        Task SaveChangesAsync();
    }
}
