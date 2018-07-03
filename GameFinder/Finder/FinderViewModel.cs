using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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

        private ICommand _gitHubCommand;

        private ICommand _reportBugCommand;


        public FinderViewModel()
        {
            var feed = MessageFeed<FriendsLoadedStruct>.Feed;
            feed.MessageReceived += OnMessageReceived;
            ReportBugCommand = new RelayCommand(ReportBugAction);
            GitHubCommand = new RelayCommand(GitHubAction);
        }

        private void GitHubAction(object o)
        {
            try
            {
                Process.Start("https://github.com/mrousavy/GameFinder");
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel("Could not open GitHub page! \n\r" +
                                                           "You can open it manually: https://github.com/mrousavy/GameFinder \n\r" +
                                                           ex.Message);
            }
        }

        private void ReportBugAction(object o)
        {
            try
            {
                Process.Start("https://github.com/mrousavy/GameFinder/issues/new");
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel("Could not open bug-report page!\n\r" +
                                                           "You can open it manually: https://github.com/mrousavy/GameFinder/issues \n\r" +
                                                           ex.Message);
            }
        }

        public ICommand ReportBugCommand
        {
            get => _reportBugCommand;
            set => Set(ref _reportBugCommand, value);
        }

        public ICommand GitHubCommand
        {
            get => _gitHubCommand;
            set => Set(ref _gitHubCommand, value);
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

            DialogViewModel = new LoadingDialogViewModel("Matching games...");
            try
            {
                // Load mutual games
                var users = new List<SteamProfile>();
                users.AddRange(profiles.Select(p => new SteamProfile(p.SteamId)));
                users.Add(new SteamProfile(you.SteamId));

                foreach (var user in users)
                {
                    user.Games = await SteamHelper.LoadGamesAsync(user.SteamId);
                    if (!user.Games.Any())
                        throw new Exception($"{user.SteamId}'s steam profile is set to private or games could not be loaded!");
                }

                var equalityComparer = new GameEqualityComparer();
                var mutualGames = users
                    .SelectMany(x => x.Games)
                    .Where(game => users.All(friend => friend.Games.Any(friendGame => friendGame.AppId == game.AppId)))
                    .Distinct(equalityComparer)
                    .ToList();
                var gameViewModels = mutualGames.Select(SteamHelper.OwnedGameToGame).OrderBy(g => g.Name);
                var games = new ObservableCollection<GameViewModel>(gameViewModels);


                // Load own profile
                MyProfileViewModel = SteamHelper.ProfileToUser(you);
                MyProfileViewModel.Games = games;

                // Load all friends
                var ordered = profiles.Select(SteamHelper.ProfileToUser).OrderBy(u => u.Username);
                Friends = new ObservableCollection<UserViewModel>(ordered);
                foreach (var friend in Friends)
                {
                    friend.Games = games;
                }

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not find matching games! Perhaps a profile is set to private?\n\r{ex.Message}");
            }
        }
    }
}