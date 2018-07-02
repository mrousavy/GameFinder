using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GameFinder.ErrorDialog;
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


        public FinderViewModel()
        {
            var feed = MessageFeed<LoggedInStruct>.Feed;
            feed.MessageReceived += OnMessageReceived;
        }

        private async void OnMessageReceived(LoggedInStruct message)
        {
            SteamUser = message.SteamUser;
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

                var me = await SteamUser.GetCommunityProfileAsync(Session.UserId);
                MyProfileViewModel = ProfileToUser(me);

                var friendsListResponse = await SteamUser.GetFriendsListAsync(Session.UserId);
                foreach (var friend in friendsListResponse.Data)
                {
                    var profile = await SteamUser.GetCommunityProfileAsync(friend.SteamId);
                    var model = ProfileToUser(profile);
                    Friends.Add(model);
                }

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel($"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }

        private static UserViewModel ProfileToUser(SteamCommunityProfileModel profile)
        {
            string url = Extensions.Valid(profile.CustomURL) ? $"http://steamcommunity.com/id/{profile.CustomURL}" : null;

            return new UserViewModel
            {
                AvatarUri = profile.Avatar,
                RealName = profile.RealName,
                Url = url,
                Username = profile.Headline,
                State = profile.StateMessage,
                VisibilityState = (int) profile.VisibilityState
            };
        }
    }
}
