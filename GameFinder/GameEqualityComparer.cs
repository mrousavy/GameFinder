using System.Collections.Generic;
using Steam.Models.SteamCommunity;

namespace GameFinder
{
    public class GameEqualityComparer : IEqualityComparer<OwnedGameModel>
    {
        public bool Equals(OwnedGameModel x, OwnedGameModel y) => x?.AppId == y?.AppId;

        public int GetHashCode(OwnedGameModel obj) => obj.AppId.GetHashCode() - 59128423;
    }
}