using System.Collections.Generic;
using Steam.Models.SteamCommunity;

namespace GameFinder
{
    public class SteamProfile
    {
        public ulong SteamId { get; set; }

        public IList<OwnedGameModel> Games { get; set; }

        public SteamProfile(ulong steamId)
        {
            SteamId = steamId;
            Games = new List<OwnedGameModel>();
        }
    }
}
