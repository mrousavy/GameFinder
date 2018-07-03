using System;
using Jellyfish;

namespace GameFinder.UserSmall
{
    public class UserSmallViewModel : ViewModel
    {
        private Uri _avatarUri;
        private ulong _userId;
        private string _username;

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


        public override bool Equals(object obj) => obj is UserSmallViewModel model &&
                                                   UserId == model.UserId;

        public override int GetHashCode() => -566744556 + UserId.GetHashCode();
    }
}