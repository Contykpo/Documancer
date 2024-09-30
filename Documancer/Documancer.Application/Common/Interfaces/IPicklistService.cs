using Documancer.Application.Features.KeyValues.DTOs;

namespace Documancer.Application.Common.Interfaces
{
    public interface IPicklistService
    {
        #region Events

        event Func<Task>? OnChange;

        #endregion

        #region Properties

        List<KeyValueDto> DataSource { get; }

        #endregion

        #region Methods

        void Initialize();
        void Refresh();

        #endregion
    }
}