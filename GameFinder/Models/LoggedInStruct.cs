using SteamWebAPI2.Interfaces;

namespace GameFinder.Models
{
    public struct LoggedInStruct
    {
        public LoggedInStruct(ISteamUser user)
        {
            SteamUser = user;
        }

        public ISteamUser SteamUser { get; }
    }
}
