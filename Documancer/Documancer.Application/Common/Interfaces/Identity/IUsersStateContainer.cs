using System.Collections.Concurrent;

namespace Documancer.Application.Common.Interfaces.Identity
{
    public interface IUsersStateContainer
    {
        #region Events

        event Action? OnChange;

        #endregion

        #region Properties

        ConcurrentDictionary<string, string> UsersByConnectionId { get; }

        #endregion

        #region Methods

        void AddOrUpdate(string connectionId, string? name);
        void Remove(string connectionId);

        #endregion
    }
}