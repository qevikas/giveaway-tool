using giveaway_tool.Contracts;
using giveaway_tool.Models;
using Microsoft.Extensions.Caching.Memory;

namespace giveaway_tool.Services;

public class WorkspaceStore(IMemoryCache _cache) : IWorkspaceStore
{
    private readonly object _sync = new();

    private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan AbsoluteTimeout = TimeSpan.FromHours(4);
    private const int MaximumParticipants = 20_000;

    public GiveawayWorkspace Create(string workspaceKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(workspaceKey, nameof(workspaceKey));

        lock (_sync)
        {
            var workspace = new GiveawayWorkspace(DateTime.UtcNow.Add(AbsoluteTimeout), MaximumParticipants);
            _cache.Set(GetCacheKey(workspaceKey), workspace, new MemoryCacheEntryOptions
            {
                SlidingExpiration = IdleTimeout,
                AbsoluteExpirationRelativeToNow = AbsoluteTimeout,
                Size = 1
            });
            return workspace;
        }
    }

    public bool Exists(string workspaceKey) => Get(workspaceKey) is not null;

    public GiveawayWorkspace? Get(string workspaceKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceKey, nameof(workspaceKey));

        if (!_cache.TryGetValue(GetCacheKey(workspaceKey), out GiveawayWorkspace? workspace)) return null;

        if (workspace is null || workspace.AbsoluteExpiresAtUtc <= DateTime.UtcNow)
        {
            _cache.Remove(GetCacheKey(workspaceKey));
            return null;
        }
        return workspace;
    }

    private static string GetCacheKey(string workspaceKey) => $"giveaway:workspace:{workspaceKey}";

}
