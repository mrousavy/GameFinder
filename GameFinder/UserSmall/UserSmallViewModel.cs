using Jellyfish;
using System;

namespace GameFinder.UserSmall
{
    public class UserSmallViewModel : ViewModel
    {
        private Uri _avatarUri;
        private string _username;
        private ulong _userId;

        public UserSmallViewModel(ulong userId)
        {
            UserId = userId;
        }

        public ulong UserId
        {
            get => _userId;
            set => Set(ref _userId, value);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public Uri AvatarUri
        {
            get => _avatarUri;
            set => Set(ref _avatarUri, value);
        }
    }
}