using Documancer.Server.Models.NavigationMenu;

namespace Documancer.Server.Services.Navigation
{
    public interface IMenuService
    {
        IEnumerable<MenuSectionModel> Features { get; }
    }
}