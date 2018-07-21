using System;
using System.Diagnostics;
using System.Windows.Input;
using Jellyfish;
using Steam.Models.SteamCommunity;

namespace GameFinder.User
{
    public class UserViewModel : ViewModel
    {
        private Uri _avatarUri;

        private ICommand _openProfileCommand;

        private string _realName;

        private UserStatus _state;

        private string _url;

        private ulong _userId;
        private string _username;

        private int _visibilityState;

        private int _matchingGames;

        private int _totalGames;

        public UserViewModel(ulong userId)
        {
            UserId = userId;
            OpenProfileCommand = new RelayCommand<string>(OpenProfileAction);
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

        public int MatchingGames
        {
            get => _matchingGames;
            set
            {
                Set(ref _matchingGames, value);
                Notify(nameof(MatchingGamesString));
            }
        }

        public int TotalGames
        {
            get => _totalGames;
            set
            {
                Set(ref _totalGames, value);
                Notify(nameof(MatchingGamesString));
            }
        }

        public string MatchingGamesString => $"{MatchingGames}/{TotalGames} matching";

        public string RealName
        {
            get => _realName;
            set => Set(ref _realName, value);
        }

        public string Url
        {
            get => _url;
            set => Set(ref _url, value);
        }

        public UserStatus State
        {
            get => _state;
            set => Set(ref _state, value);
        }

        public int VisibilityState
        {
            get => _visibilityState;
            set => Set(ref _visibilityState, value);
        }

        public Uri AvatarUri
        {
            get => _avatarUri;
            set => Set(ref _avatarUri, value);
        }

        public ICommand OpenProfileCommand
        {
            get => _openProfileCommand;
            set => Set(ref _openProfileCommand, value);
        }

        private static void OpenProfileAction(string url)
        {
            try
            {
                Process.Start(url);
            } catch
            {
                // could not open profile
            }
        }

        public override bool Equals(object obj) => obj is UserViewModel model &&
                                                   UserId == model.UserId;

        public override int GetHashCode() => UserId.GetHashCode() - 566744556;
    }
}