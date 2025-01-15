namespace EventsWebApp.API.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IEventRepository Events { get; }
        IParticipantRepository Participants { get; }
        Task<int> SaveChangesAsync();
    }
}
