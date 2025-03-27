namespace BlueBerry24.Services.NotificationAPI.Services.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
