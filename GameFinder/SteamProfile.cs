using System.Collections.Generic;
using Steam.Models.SteamCommunity;

namespace GameFinder
{
    public class SteamProfile
    {
        public SteamProfile(ulong steamId)
        {
            SteamId = steamId;
            Games = new List<OwnedGameModel>();
        }

        public ulong SteamId { get; set; }

        public IList<OwnedGameModel> Games { get; set; }
    }
}