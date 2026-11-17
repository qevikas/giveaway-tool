using giveaway_tool.Models;

namespace giveaway_tool.Contracts;
public interface IWorkspaceStore
{
    GiveawayWorkspace Create(string workspaceKey);
    bool Exists(string workspaceKey);
    GiveawayWorkspace? Get(string workspaceKey);
}
