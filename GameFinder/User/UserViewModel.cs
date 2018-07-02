using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using GameFinder.Game;
using Jellyfish;

namespace GameFinder.User
{
    public class UserViewModel : ViewModel
    {
        private Uri _avatarUri;

        private IEnumerable<GameViewModel> _games;

        private ICommand _openProfileCommand;

        private string _realName;

        private string _state;

        private string _url;
        private string _username;

        private int _visibilityState;

        public UserViewModel()
        {
            OpenProfileCommand = new RelayCommand<string>(OpenProfileAction);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

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

        public string State
        {
            get => _state;
            set => Set(ref _state, value);
        }

        public int VisibilityState
        {
            get => _visibilityState;
            set => Set(ref _visibilityState, value);
        }

        public IEnumerable<GameViewModel> Games
        {
            get => _games;
            set => Set(ref _games, value);
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
    }
}