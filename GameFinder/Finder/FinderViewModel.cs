using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GameFinder.ErrorDialog;
using GameFinder.Game;
using GameFinder.LoadingDialog;
using GameFinder.User;
using Jellyfish;
using Steam.Models.SteamCommunity;

namespace GameFinder.Finder
{
    public class FinderViewModel : ViewModel
    {
        private object _dialogViewModel;

        private ObservableCollection<UserViewModel> _friends;

        private bool _isDialogOpen;

        private UserViewModel _myProfileViewModel;


        public FinderViewModel()
        {
            var feed = MessageFeed<FriendsLoadedStruct>.Feed;
            feed.MessageReceived += OnMessageReceived;
        }


        public object DialogViewModel
        {
            get => _dialogViewModel;
            set
            {
                Set(ref _dialogViewModel, value);
                IsDialogOpen = value != null;
            }
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
        }

        public UserViewModel MyProfileViewModel
        {
            get => _myProfileViewModel;
            set => Set(ref _myProfileViewModel, value);
        }

        public ObservableCollection<UserViewModel> Friends
        {
            get => _friends;
            set => Set(ref _friends, value);
        }

        private async void OnMessageReceived(FriendsLoadedStruct message)
        {
            await Load(message.You, message.Friends);
        }

        private async Task Load(PlayerSummaryModel you, IList<PlayerSummaryModel> profiles)
        {
            if (Session.SteamUser == null)
                return;

            DialogViewModel = new LoadingDialogViewModel();
            try
            {
                // Load mutual games
                var users = new List<SteamProfile>();
                users.AddRange(profiles.Select(p => new SteamProfile(p.SteamId)));
                users.Add(new SteamProfile(you.SteamId));

                foreach (var user in users)
                {
                    user.Games = await SteamHelper.LoadGamesAsync(user.SteamId);
                }

                var equalityComparer = new GameEqualityComparer();
                var mutualGames = users
                    .SelectMany(x => x.Games)
                    .Where(game => users.All(friend => friend.Games.Any(friendGame => friendGame.AppId == game.AppId)))
                    .Distinct(equalityComparer)
                    .ToList();
                var gameViewModels = mutualGames.Select(SteamHelper.OwnedGameToGame);
                var games = new ObservableCollection<GameViewModel>(gameViewModels);


                // Load own profile
                MyProfileViewModel = SteamHelper.ProfileToUser(you);
                MyProfileViewModel.Games = games;

                // Load all friends
                Friends = new ObservableCollection<UserViewModel>(profiles.Select(SteamHelper.ProfileToUser));
                foreach(var friend in Friends)
                {
                    friend.Games = games;
                }

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }
    }
}