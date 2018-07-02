using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameFinder.ErrorDialog;
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

        private void OnMessageReceived(FriendsLoadedStruct message)
        {
            Load(message.You, message.Friends);
        }

        private void Load(SteamCommunityProfileModel you, IEnumerable<SteamCommunityProfileModel> profiles)
        {
            if (Session.SteamUser == null)
                return;

            DialogViewModel = new LoadingDialogViewModel();
            try
            {
                Friends = new ObservableCollection<UserViewModel>(profiles.Select(SteamHelper.ProfileToUser));
                MyProfileViewModel = SteamHelper.ProfileToUser(you);

                IsDialogOpen = false;
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel(
                    $"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
            }
        }
    }
}