using System.Collections.Concurrent;

namespace Documancer.Infrastructure.Services.Identity
{
    public class UsersStateContainer : IUsersStateContainer
    {
        #region Events

        public event Action? OnChange;

        #endregion

        #region Properties

        public ConcurrentDictionary<string, string> UsersByConnectionId { get; } = new();

        #endregion

        #region Methods

        public void AddOrUpdate(string connectionId, string? name)
        {
            UsersByConnectionId.AddOrUpdate(connectionId, name ?? string.Empty, (key, oldValue) => name ?? string.Empty);
            NotifyStateChanged();
        }

        public void Remove(string connectionId)
        {
            UsersByConnectionId.TryRemove(connectionId, out var _);
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        #endregion
    }
}