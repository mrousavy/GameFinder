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
using Jellyfish.Feeds;
using Steam.Models.SteamCommunity;

namespace GameFinder.Finder
{
    public class FinderViewModel : ViewModel, INode<FriendsLoadedStruct>
    {
        private readonly Random _random;
        private object _dialogViewModel;

        private ObservableCollection<UserViewModel> _friends;

        private ObservableCollection<GameViewModel> _games;

        private ICommand _gitHubCommand;

        private bool _isDialogOpen;

        private ICommand _launchRandomGameCommand;

        private UserViewModel _myProfileViewModel;

        private ICommand _reportBugCommand;

        private int _tileColumns;

        public FinderViewModel()
        {
            var feed = Feed<FriendsLoadedStruct>.Instance;
            feed.RegisterNode(this);
            ReportBugCommand = new RelayCommand(ReportBugAction);
            GitHubCommand = new RelayCommand(GitHubAction);
            LaunchRandomGameCommand = new RelayCommand(LaunchRandomGameAction, o => Games?.Count > 0);
            _random = new Random();
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

        public ICommand LaunchRandomGameCommand
        {
            get => _launchRandomGameCommand;
            set => Set(ref _launchRandomGameCommand, value);
        }

        public int TileColumns
        {
            get => _tileColumns;
            set => Set(ref _tileColumns, value);
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

        public ObservableCollection<GameViewModel> Games
        {
            get => _games;
            set => Set(ref _games, value);
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

        private void LaunchRandomGameAction(object o)
        {
            try
            {
                int count = Games?.Count ?? 0;
                int random = _random.Next(0, count);
                var game = Games?[random];

                if (game == null)
                {
                    DialogViewModel = new ErrorDialogViewModel("A random game could not be selected!");
                } else
                {
                    if (game.LaunchCommand.CanExecute(o))
                    {
                        game.LaunchCommand.Execute(o);
                    }
                }
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel("Could not launch a random game!\n\r" +
                                                           "Are you sure Steam is installed on this machine and links starting with \"steam://\" are registered? \n\r" +
                                                           ex.Message);
            }
        }

        public async void MessageReceived(FriendsLoadedStruct message)
        {
            await Load(message.You, message.Friends);
        }

        private async Task Load(PlayerSummaryModel you, ICollection<PlayerSummaryModel> profiles)
        {
            if (Session.SteamUser == null)
            {
                return;
            }

            DiscordHelper.SetPresence(profiles.Count > 1
                ? $"Matching {profiles.Count} profiles"
                : $"Matching {profiles.Count} profile");

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
                    {
                        throw new Exception(
                            $"{user.SteamId}'s steam profile is set to private or games could not be loaded!");
                    }
                }



                var equalityComparer = new GameEqualityComparer();
                var mutualGames = users
                    .SelectMany(x => x.Games)
                    .Where(game => users.All(friend => friend.Games.Any(friendGame => friendGame.AppId == game.AppId)))
                    .Distinct(equalityComparer)
                    .OrderBy(g => g.Name)
                    .ToList();
                var gameViewModels = mutualGames.Select(SteamHelper.OwnedGameToGame);
                Games = new ObservableCollection<GameViewModel>(gameViewModels);

                // Load own profile
                MyProfileViewModel = SteamHelper.ProfileToUser(you);

                // Load all friends
                var ordered = profiles.Select(SteamHelper.ProfileToUser).OrderBy(u => u.Username);
                Friends = new ObservableCollection<UserViewModel>(ordered);

                DiscordHelper.SetPresence($"Browsing {Games.Count} games..");

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not find matching games! Perhaps a profile is set to private?\n\r{ex.Message}");
            }
        }
    }
}