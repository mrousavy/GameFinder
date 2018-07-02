﻿using System;
using System.Collections.Generic;
using Jellyfish;
using Steam.Models.SteamCommunity;

namespace GameFinder.User
{
    public class UserViewModel : ViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        private string _realName;
        public string RealName
        {
            get => _realName;
            set => Set(ref _realName, value);
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => Set(ref _url, value);
        }

        private IEnumerable<OwnedGameModel> _games;
        public IEnumerable<OwnedGameModel> Games
        {
            get => _games;
            set => Set(ref _games, value);
        }

        private Uri _avatarUri;
        public Uri AvatarUri
        {
            get => _avatarUri;
            set => Set(ref _avatarUri, value);
        }
    }
}