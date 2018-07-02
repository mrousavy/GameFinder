using Jellyfish;
using SteamWebAPI2.Interfaces;

namespace GameFinder
{
    public static class Session
    {
        public static Config Config { get; set; }


        public static string ApiKey
        {
            get => Config.ApiKey;
            set => Config.ApiKey = value;
        }
        public static long UserId
        {
            get => Config.UserId;
            set => Config.UserId = value;
        }
    }
}
