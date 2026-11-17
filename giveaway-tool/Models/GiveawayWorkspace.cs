namespace giveaway_tool.Models;
public class GiveawayWorkspace
{
    private int _participantCount;

    public Guid ID { get; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    public DateTime AbsoluteExpiresAtUtc { get; }
    public int MaximumParticipants { get; }
    public int ParticipantCount => Volatile.Read(ref _participantCount);

    public GiveawayWorkspace(DateTime absoluteExpiresAtUtc, int maximumParticipants)
    {
        if (maximumParticipants <= 0) throw new ArgumentOutOfRangeException(nameof(maximumParticipants), "Maximum participants must be greater than zero.");
        AbsoluteExpiresAtUtc = absoluteExpiresAtUtc;
        MaximumParticipants = maximumParticipants;
    }

    public bool TryReserveParticipants(int count)
    {
        if (count <= 0) return false;
        while (true)
        {
            var current = Volatile.Read(ref _participantCount);
            if (count > (MaximumParticipants - current)) return false;
            var updated = current + count;
            if (Interlocked.CompareExchange(ref _participantCount, updated, current) == current) return true;
        }
    }
}
