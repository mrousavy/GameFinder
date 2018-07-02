namespace GameFinder
{
    public static class Session
    {
        public static Config Config { get; set; }

        public static string ApiKey
        {
            get => Config?.ApiKey;
            set
            {
                if (Config != null)
                    Config.ApiKey = value;
            }
        }
        public static ulong UserId
        {
            get => Config?.UserId ?? 0;
            set
            {
                if (Config != null)
                    Config.UserId = value;
            }
        }
    }
}
