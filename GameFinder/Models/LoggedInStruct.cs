using SteamWebAPI2.Interfaces;

namespace GameFinder.Models
{
    public struct LoggedInStruct
    {
        public LoggedInStruct(ISteamUser user, IPlayerService player)
        {
            SteamUser = user;
            SteamPlayer = player;
        }

        public ISteamUser SteamUser { get; }
        public IPlayerService SteamPlayer { get; }
    }
}