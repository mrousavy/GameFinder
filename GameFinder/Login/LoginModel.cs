using System.Net.Http;
using Jellyfish;
using Jellyfish.Feeds;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace GameFinder.Login
{
    public class LoginModel
    {
        public void Login(string apiKey, string userId)
        {
            Session.ApiKey = apiKey;
            Session.UserId = ulong.Parse(userId);

            var webInterfaceFactory = new SteamWebInterfaceFactory(apiKey);
            Session.SteamUser = webInterfaceFactory.CreateSteamWebInterface<SteamUser>(new HttpClient());
            Session.SteamPlayer = webInterfaceFactory.CreateSteamWebInterface<PlayerService>(new HttpClient());
            var feed = Feed<bool>.Instance;
            feed.Notify(true);

            Extensions.MoveForwards();
        }
    }
}