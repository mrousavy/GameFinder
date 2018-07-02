using System;
using System.Collections.ObjectModel;
using GameFinder.ErrorDialog;
using GameFinder.LoadingDialog;
using GameFinder.Models;
using GameFinder.User;
using Jellyfish;
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

        private void OnMessageReceived(LoggedInStruct message)
        {
            SteamUser = message.SteamUser;
            LoadFriends();
        }

        private async void LoadFriends()
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

                var friendsListResponse = await SteamUser.GetFriendsListAsync((ulong) Session.UserId);
                foreach (var friend in friendsListResponse.Data)
                {
                    var profile = await SteamUser.GetCommunityProfileAsync(friend.SteamId);
                    var model = new UserViewModel
                    {
                        AvatarUri = profile.Avatar,
                        RealName = profile.RealName,
                        Url = $"http://steamcommunity.com/id/{profile.CustomURL}",
                        Username = profile.Headline,
                        State = profile.State
                    };
                    Friends.Add(model);
                }

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel($"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }
    }
}
