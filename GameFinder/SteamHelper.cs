using GameFinder.Game;
using Steam.Models.SteamCommunity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameFinder.User;
using GameFinder.UserSmall;

namespace GameFinder
{
    public static class SteamHelper
    {

        public static async Task<IEnumerable<GameViewModel>> LoadGamesAsync(ulong userId)
        {
            var gamesResponse = await Session.SteamPlayer.GetOwnedGamesAsync(userId, true, false);
            var ownedGames = gamesResponse.Data.OwnedGames;
            var games = ownedGames?.Select(OwnedGameToGame);
            return games;
        }

        private static GameViewModel OwnedGameToGame(OwnedGameModel game)
        {
            if (game == null)
                return null;

            string url = Extensions.Valid(game.ImgLogoUrl)
                ? $"http://media.steampowered.com/steamcommunity/public/images/apps/{game.AppId}/{game.ImgLogoUrl}.jpg"
                : null;

            return new GameViewModel
            {
                IconUrl = url,
                Name = game.Name
            };
        }

        public static Task<SteamCommunityProfileModel> GetProfile(ulong steamId) =>
            Session.SteamUser.GetCommunityProfileAsync(steamId);

        public static async Task<IList<SteamCommunityProfileModel>> GetFriends()
        {
            IList<SteamCommunityProfileModel> friends = new List<SteamCommunityProfileModel>();

            var friendsListResponse = await Session.SteamUser.GetFriendsListAsync(Session.UserId).ConfigureAwait(false);
            foreach (var friend in friendsListResponse.Data)
            {
                var profile = await GetProfile(friend.SteamId).ConfigureAwait(false);
                friends.Add(profile);
            }

            return friends;
        }

        public static UserSmallViewModel ProfileToUserSmall(SteamCommunityProfileModel profile)
        {
            if (profile == null)
                return null;

            return new UserSmallViewModel(profile.SteamID)
            {
                AvatarUri = profile.Avatar,
                Username = profile.RealName
            };
        }

        public static UserViewModel ProfileToUser(SteamCommunityProfileModel profile)
        {
            if (profile == null)
                return null;

            string url = Extensions.Valid(profile.CustomURL)
                ? $"http://steamcommunity.com/id/{profile.CustomURL}"
                : null;

            return new UserViewModel(profile.SteamID)
            {
                AvatarUri = profile.Avatar,
                RealName = profile.RealName,
                Url = url,
                Username = profile.RealName,
                State = profile.StateMessage,
                VisibilityState = (int) profile.VisibilityState
            };
        }
    }
}
