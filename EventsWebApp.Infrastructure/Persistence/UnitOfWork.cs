using EventsWebApp.Application.Interfaces;

namespace EventsWebApp.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public IEventRepository Events { get; }
        public IParticipantRepository Participants { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            IEventRepository eventRepository,
            IParticipantRepository participantRepository)
        {
            _context = context;
            Users = userRepository;
            Events = eventRepository;
            Participants = participantRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}