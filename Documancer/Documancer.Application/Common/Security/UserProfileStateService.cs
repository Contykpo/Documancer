namespace Documancer.Application.Common.Security
{
    public class UserProfileStateService
    {
        #region Events

        public event Action? OnChange;

        #endregion

        #region Properties and Fields

        private UserProfile _userProfile = new UserProfile() { Email = "", UserId = "", UserName = "" };

        #endregion

        #region Constructors

        public UserProfile UserProfile
        {
            get => _userProfile;
            set
            {
                _userProfile = value;
                NotifyStateChanged();
            }
        }

        #endregion

        #region Methods

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void UpdateUserProfile(string? profilePictureDataUrl, string? fullName, string? phoneNumber)
        {
            _userProfile.ProfilePictureDataUrl = profilePictureDataUrl;
            _userProfile.DisplayName = fullName;
            _userProfile.PhoneNumber = phoneNumber;

            NotifyStateChanged();
        }

        #endregion
    }
}