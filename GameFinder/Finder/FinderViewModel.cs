using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GameFinder.ErrorDialog;
using GameFinder.Game;
using GameFinder.LoadingDialog;
using GameFinder.Models;
using GameFinder.User;
using Jellyfish;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;

namespace GameFinder.Finder
{
    public class FinderViewModel : ViewModel
    {
        private object _dialogViewModel;
        public object DialogViewModel
        {
            get => _dialogViewModel;
            set
            {
                Set(ref _dialogViewModel, value);
                IsDialogOpen = value != null;
            }
        }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
        }

        private UserViewModel _myProfileViewModel;
        public UserViewModel MyProfileViewModel
        {
            get => _myProfileViewModel;
            set => Set(ref _myProfileViewModel, value);
        }

        private ObservableCollection<UserViewModel> _friends;
        public ObservableCollection<UserViewModel> Friends
        {
            get => _friends;
            set => Set(ref _friends, value);
        }

        public ISteamUser SteamUser { get; set; }
        public IPlayerService SteamPlayer { get; set; }


        public FinderViewModel()
        {
            var feed = MessageFeed<LoggedInStruct>.Feed;
            feed.MessageReceived += OnMessageReceived;
        }

        private async void OnMessageReceived(LoggedInStruct message)
        {
            SteamUser = message.SteamUser;
            SteamPlayer = message.SteamPlayer;
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            if (SteamUser == null)
                return;

            DialogViewModel = new LoadingDialogViewModel();
            try
            {
                if (Friends == null)
                    Friends = new ObservableCollection<UserViewModel>();
                else
                    Friends.Clear();
                
                MyProfileViewModel = await GetUser(Session.UserId);

                var friendsListResponse = await SteamUser.GetFriendsListAsync(Session.UserId);
                foreach (var friend in friendsListResponse.Data)
                {
                    var model = await GetUser(friend.SteamId);
                    Friends.Add(model);
                }

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel($"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }

        private async Task<UserViewModel> GetUser(ulong steamId)
        {
            var profile = await SteamUser.GetCommunityProfileAsync(steamId);
            var gamesResponse = await SteamPlayer.GetOwnedGamesAsync(steamId, false, true);
            var games = gamesResponse.Data.OwnedGames;

            return ProfileToUser(profile, games);
        }

        private static UserViewModel ProfileToUser(SteamCommunityProfileModel profile, IEnumerable<OwnedGameModel> ownedGames)
        {
            if (profile == null) return null;

            string url = Extensions.Valid(profile.CustomURL) ? $"http://steamcommunity.com/id/{profile.CustomURL}" : null;
            var games = ownedGames?.Select(OwnedGameToGame);

            return new UserViewModel
            {
                AvatarUri = profile.Avatar,
                RealName = profile.RealName,
                Url = url,
                Username = profile.Headline,
                State = profile.StateMessage,
                VisibilityState = (int) profile.VisibilityState,
                Games = games
            };
        }

        private static GameViewModel OwnedGameToGame(OwnedGameModel game)
        {
            if (game == null) return null;

            return new GameViewModel
            {
                IconUrl = game.ImgLogoUrl,
                Name = game.Name,
                Playtime = game.PlaytimeForever
            };
        }
    }
}
