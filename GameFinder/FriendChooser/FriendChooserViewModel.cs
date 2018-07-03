using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GameFinder.ErrorDialog;
using GameFinder.LoadingDialog;
using GameFinder.UserSmall;
using Jellyfish;
using MaterialDesignThemes.Wpf.Transitions;
using Steam.Models.SteamCommunity;

namespace GameFinder.FriendChooser
{
    public class FriendChooserViewModel : ViewModel
    {
        private ObservableCollection<UserSmallViewModel> _allFriends;

        private ObservableCollection<UserSmallViewModel> _chosenFriends;
        private object _dialogViewModel;
        private bool _isDialogOpen;

        private ICommand _okCommand;


        public FriendChooserViewModel()
        {
            var feed = MessageFeed<bool>.Feed;
            feed.MessageReceived += OnMessageReceived;
            AllFriends = new ObservableCollection<UserSmallViewModel>();
            ChosenFriends = new ObservableCollection<UserSmallViewModel>();
            OkCommand = new RelayCommand(OkAction);
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

                var feed = MessageFeed<FriendsLoadedStruct>.Feed;
                feed.Notify(new FriendsLoadedStruct(profiles, you));

                IsDialogOpen = false;
                Transitioner.MoveNextCommand.Execute(null, null);
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }


        private async void OnMessageReceived(bool loggedIn)
        {
            await LoadAsync().ConfigureAwait(false);
        }

        private async Task LoadAsync()
        {
            if (Session.SteamUser == null)
                return;

            DialogViewModel = new LoadingDialogViewModel("Loading friends...");
            try
            {
                var friends = await SteamHelper.GetFriends();
                AllFriends =
                    new ObservableCollection<UserSmallViewModel>(friends.Select(SteamHelper.ProfileToUserSmall));

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }
    }
}