using Jellyfish;

namespace GameFinder
{
    public class Config : Preferences
    {
        public Config(string path) : base(path)
        { }

        public string ApiKey { get; set; }
        public ulong UserId { get; set; }
    }
}
