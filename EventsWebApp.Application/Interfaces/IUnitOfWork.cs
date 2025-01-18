namespace EventsWebApp.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IEventRepository Events { get; }
        IParticipantRepository Participants { get; }
    }
}
