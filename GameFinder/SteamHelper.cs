using System;
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

        public static async Task<IList<OwnedGameModel>> LoadGamesAsync(ulong userId)
        {
            var gamesResponse = await Session.SteamPlayer.GetOwnedGamesAsync(userId, true, false);
            return gamesResponse.Data.OwnedGames.ToList();
            //var games = ownedGames.Select(OwnedGameToGame);
            //return games.ToList();
        }

        public static GameViewModel OwnedGameToGame(OwnedGameModel game)
        {
            if (game == null)
                return null;

            string url = Extensions.Valid(game.ImgLogoUrl)
                ? $"http://media.steampowered.com/steamcommunity/public/images/apps/{game.AppId}/{game.ImgLogoUrl}.jpg"
                : null;

            return new GameViewModel(game.AppId)
            {
                IconUrl = url,
                Name = game.Name
            };
        }

        public static async Task<PlayerSummaryModel> GetProfile(ulong steamId)
        {
            var response = await Session.SteamUser.GetPlayerSummaryAsync(steamId);
            return response.Data;
        }

        public static async Task<IList<PlayerSummaryModel>> GetFriends()
        {
            IList<PlayerSummaryModel> friends = new List<PlayerSummaryModel>();

            var friendsListResponse = await Session.SteamUser.GetFriendsListAsync(Session.UserId).ConfigureAwait(false);
            foreach (var friend in friendsListResponse.Data)
            {
                var profile = await GetProfile(friend.SteamId).ConfigureAwait(false);
                friends.Add(profile);
            }

            return friends;
        }

        public static UserSmallViewModel ProfileToUserSmall(PlayerSummaryModel profile)
        {
            if (profile == null)
                return null;

            return new UserSmallViewModel(profile.SteamId)
            {
                AvatarUri = new Uri(profile.AvatarUrl, UriKind.Absolute),
                Username = profile.Nickname
            };
        }

        public static UserViewModel ProfileToUser(PlayerSummaryModel profile)
        {
            if (profile == null)
                return null;

            return new UserViewModel(profile.SteamId)
            {
                AvatarUri = new Uri(profile.AvatarUrl, UriKind.Absolute),
                RealName = profile.RealName,
                Url = profile.ProfileUrl,
                Username = profile.Nickname,
                State = profile.UserStatus
            };
        }
    }
}
