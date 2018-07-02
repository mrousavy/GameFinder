using System;
using System.Collections.Generic;
using Steam.Models.SteamCommunity;

namespace GameFinder
{
    public struct FriendsLoadedStruct
    {
        public FriendsLoadedStruct(IList<PlayerSummaryModel> friends, PlayerSummaryModel you)
        {
            Friends = friends ?? throw new ArgumentNullException(nameof(friends));
            You = you ?? throw new ArgumentNullException(nameof(you));
        }

        public IList<PlayerSummaryModel> Friends { get; }
        public PlayerSummaryModel You { get; }
    }
}