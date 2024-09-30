using Documancer.Application.Features.Identity.DTOs;

namespace Documancer.Application.Common.Interfaces.Identity
{
    public interface IUserService
    {
        #region Events

        event Func<Task>? OnChange;

        #endregion

        #region Properties

        List<ApplicationUserDto> DataSource { get; }

        #endregion

        #region Methods

        void Initialize();
        void Refresh();

        #endregion
    }
}