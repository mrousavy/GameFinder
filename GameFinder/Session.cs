using SteamWebAPI2.Interfaces;

namespace GameFinder
{
    public static class Session
    {
        public static string ApiKey { get; set; }
        public static long UserId { get; set; }
        public static ISteamUser SteamUser { get; set; }
    }
}
