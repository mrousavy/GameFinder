using Jellyfish;

namespace GameFinder
{
    public class Config : Preferences
    {
        public Config(string path) : base(path)
        { }

        public string ApiKey { get; set; }
        public long UserId { get; set; }
    }
}
