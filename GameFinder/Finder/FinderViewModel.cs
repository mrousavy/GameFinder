using System;
using System.Collections.ObjectModel;
using GameFinder.ErrorDialog;
using GameFinder.Models;
using GameFinder.User;
using Jellyfish;

namespace GameFinder.Finder
{
    public class FinderViewModel : ViewModel
    {
        private object _dialogViewModel;
        public object DialogViewModel
        {
            get => _dialogViewModel;
            set => Set(ref _dialogViewModel, value);
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

        public FinderModel Model { get; set; }


        public FinderViewModel()
        {
            Model = new FinderModel();

            var feed = MessageFeed<LoggedInStruct>.Feed;
            feed.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(LoggedInStruct message)
        {
            Model.SteamUser = message.SteamUser;
            LoadFriends();
        }

        private async void LoadFriends()
        {
            try
            {
                var friends = await Model.GetFriends(Session.UserId);
                Friends = new ObservableCollection<UserViewModel>(friends);
            } catch (Exception ex)
            {
                DialogViewModel = new ErrorDialogViewModel($"Could not load friends! Check your API Key, User ID and profile visibility!\n\r{ex.Message}");
                IsDialogOpen = true;
            }
        }
    }
}
