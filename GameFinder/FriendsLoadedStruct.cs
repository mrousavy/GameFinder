using System;
using System.Collections.Generic;
using Steam.Models.SteamCommunity;

namespace GameFinder
{
    public struct FriendsLoadedStruct
    {
        public FriendsLoadedStruct(IEnumerable<SteamCommunityProfileModel> friends, SteamCommunityProfileModel you)
        {
            Friends = friends ?? throw new ArgumentNullException(nameof(friends));
            You = you ?? throw new ArgumentNullException(nameof(you));
        }

        public IEnumerable<SteamCommunityProfileModel> Friends { get; }
        public SteamCommunityProfileModel You { get; }
    }
}