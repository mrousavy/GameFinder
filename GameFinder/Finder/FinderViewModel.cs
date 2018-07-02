using System;
using GameFinder.Models;
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
            set => Set(ref _dialogViewModel, value);
        }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
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
        }
    }
}
