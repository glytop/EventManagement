namespace EventsWebApp.Domain
{
    public interface IParticipantRepository
    {
        Task<Participant?> RegisterParticipantAsync(Participant participant);
        Task<List<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<Participant?> GetParticipantByIdAsync(int participantId);
        Task<bool> CancelParticipationAsync(int participantId);

    }
}
