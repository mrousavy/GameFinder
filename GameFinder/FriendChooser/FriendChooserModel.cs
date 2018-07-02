using System.Collections.Generic;
using System.Threading.Tasks;
using GameFinder.UserSmall;
using Steam.Models.SteamCommunity;

namespace GameFinder.FriendChooser
{
    public class FriendChooserModel
    {
        public static Task<SteamCommunityProfileModel> GetProfile(ulong steamId)
        {
            return Session.SteamUser.GetCommunityProfileAsync(steamId);
        }

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

        public static UserSmallViewModel ProfileToUser(SteamCommunityProfileModel profile)
        {
            if (profile == null)
                return null;

            return new UserSmallViewModel(profile.SteamID)
            {
                AvatarUri = profile.Avatar,
                Username = profile.Headline
            };
        }
    }
}
