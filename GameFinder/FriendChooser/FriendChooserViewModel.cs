using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GameFinder.ErrorDialog;
using GameFinder.LoadingDialog;
using GameFinder.UserSmall;
using Jellyfish;
using Jellyfish.Feeds;
using Steam.Models.SteamCommunity;

namespace GameFinder.FriendChooser
{
    public class FriendChooserViewModel : ViewModel, INode<bool>
    {
        private ObservableCollection<UserSmallViewModel> _allFriends;

        private ObservableCollection<UserSmallViewModel> _chosenFriends;
        private object _dialogViewModel;
        private bool _isDialogOpen;

        private ICommand _okCommand;


        public FriendChooserViewModel()
        {
            var feed = Feed<bool>.Instance;
            feed.RegisterNode(this);
            AllFriends = new ObservableCollection<UserSmallViewModel>();
            ChosenFriends = new ObservableCollection<UserSmallViewModel>();
            OkCommand = new RelayCommand(OkAction, o => ChosenFriends.Any());
            DiscordHelper.SetPresence("Selecting friends");
            ChosenFriends.CollectionChanged += ChosenFriendsCollectionChanged;
        }

        public ObservableCollection<UserSmallViewModel> AllFriends
        {
            get => _allFriends;
            set => Set(ref _allFriends, value);
        }

        public ObservableCollection<UserSmallViewModel> ChosenFriends
        {
            get => _chosenFriends;
            set => Set(ref _chosenFriends, value);
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

        public ICommand OkCommand
        {
            get => _okCommand;
            set => Set(ref _okCommand, value);
        }

        private void ChosenFriendsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string presence = $"Selected {ChosenFriends.Count} friend";
            if (ChosenFriends.Count != 1)
            {
                presence += "s";
            }

            DiscordHelper.SetPresence(presence);
        }

        private async void OkAction(object o)
        {
            DialogViewModel = new LoadingDialogViewModel("Loading games...");
            try
            {
                var you = await SteamHelper.GetProfile(Session.UserId);

                IList<PlayerSummaryModel> profiles = new List<PlayerSummaryModel>();
                foreach (var friend in ChosenFriends)
                {
                    var profile = await SteamHelper.GetProfile(friend.UserId);
                    profiles.Add(profile);
                }

                var feed = Feed<FriendsLoadedStruct>.Instance;
                feed.Notify(new FriendsLoadedStruct(profiles, you));

                IsDialogOpen = false;
                Extensions.MoveForwards();
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not load selected profiles! Perhaps a profile is set to private?\n\r{ex.Message}");
            }
        }


        public async void MessageReceived(bool loggedIn)
        {
            await LoadAsync().ConfigureAwait(false);
        }

        private async Task LoadAsync()
        {
            if (Session.SteamUser == null)
            {
                return;
            }

            DialogViewModel = new LoadingDialogViewModel("Loading friends...");
            try
            {
                var friends = await SteamHelper.GetFriends();
                var ordered = friends.Select(SteamHelper.ProfileToUserSmall).OrderBy(f => f.Username);
                AllFriends = new ObservableCollection<UserSmallViewModel>(ordered);

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }
    }
}