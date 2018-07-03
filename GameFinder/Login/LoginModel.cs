using Jellyfish;
using SteamWebAPI2.Interfaces;

namespace GameFinder.Login
{
    public class LoginModel
    {
        public void Login(string apiKey, string userId)
        {
            Session.ApiKey = apiKey;
            Session.UserId = ulong.Parse(userId);

            Session.SteamUser = new SteamUser(apiKey);
            Session.SteamPlayer = new PlayerService(apiKey);
            var feed = MessageFeed<bool>.Feed;
            feed.Notify(true);

            Extensions.MoveForwards();
        }
    }
}