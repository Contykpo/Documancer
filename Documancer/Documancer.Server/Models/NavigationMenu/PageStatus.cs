using System.ComponentModel;

namespace Documancer.Server.Models.NavigationMenu
{
    public enum PageStatus
    {
        [Description("Coming Soon")] ComingSoon,
        [Description("WIP")] Wip,
        [Description("New")] New,
        [Description("Completed")] Completed
    }
}