namespace BlueBerry24.Services.StockAPI.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
