using GameFinder.Models;
using Jellyfish;
using MaterialDesignThemes.Wpf.Transitions;
using SteamWebAPI2.Interfaces;

namespace GameFinder.Login
{
    public class LoginModel
    {
        public void Login(string apiKey, string userId)
        {
            Session.ApiKey = apiKey;
            Session.UserId = ulong.Parse(userId);

            var user = new SteamUser(apiKey);
            var feed = MessageFeed<LoggedInStruct>.Feed;
            feed.Notify(new LoggedInStruct(user));

            Transitioner.MoveNextCommand.Execute(null, null);
        }
    }
}
