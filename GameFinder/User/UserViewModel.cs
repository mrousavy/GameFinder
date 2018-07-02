using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GameFinder.Game;
using Jellyfish;
using Steam.Models.SteamCommunity;

namespace GameFinder.User
{
    public class UserViewModel : ViewModel
    {
        private Uri _avatarUri;

        private ObservableCollection<GameViewModel> _games;

        private ICommand _openProfileCommand;

        private string _realName;

        private string _state;

        private string _url;
        private string _username;

        private int _visibilityState;

        private ulong _userId;

        public UserViewModel(ulong userId)
        {
            UserId = userId;
            OpenProfileCommand = new RelayCommand<string>(OpenProfileAction);
            LoadGamesCommand = new RelayCommand(LoadGamesAction);
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

        public ObservableCollection<GameViewModel> Games
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

        private ICommand _loadGamesCommand;

        public ICommand LoadGamesCommand
        {
            get => _loadGamesCommand;
            set => Set(ref _loadGamesCommand, value);
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

        private async void LoadGamesAction(object o)
        {
            await LoadGamesAsync().ConfigureAwait(false);
        }

        public async Task LoadGamesAsync()
        {
            try
            {
                var gamesResponse = await Session.SteamPlayer.GetOwnedGamesAsync(UserId, true, false);
                var ownedGames = gamesResponse.Data.OwnedGames;
                var games = ownedGames?.Select(OwnedGameToGame);

                if (games != null)
                    Games = new ObservableCollection<GameViewModel>(games);
            } catch (Exception ex)
            {
                Debug.WriteLine($"Could not load games! {ex.Message}");
            }
        }

        private static GameViewModel OwnedGameToGame(OwnedGameModel game)
        {
            if (game == null)
                return null;

            string url = Extensions.Valid(game.ImgLogoUrl)
                ? $"http://media.steampowered.com/steamcommunity/public/images/apps/{game.AppId}/{game.ImgLogoUrl}.jpg"
                : null;

            return new GameViewModel
            {
                IconUrl = url,
                Name = game.Name,
                Playtime = game.PlaytimeForever
            };
        }
    }
}