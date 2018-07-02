using System.Collections.Generic;
using System.Threading.Tasks;
using GameFinder.User;
using SteamWebAPI2.Interfaces;

namespace GameFinder.Finder
{
    public class FinderModel
    {
        public ISteamUser SteamUser { get; set; }

        public async Task<IEnumerable<UserViewModel>> GetFriends(long steamId)
        {
            if (SteamUser == null) return null;

            var friendsListResponse = await SteamUser.GetFriendsListAsync((ulong)steamId);

            IList<UserViewModel> friends = new List<UserViewModel>();
            foreach (var friend in friendsListResponse.Data)
            {
                var profile = await SteamUser.GetCommunityProfileAsync(friend.SteamId);
                var model = new UserViewModel
                {
                    AvatarUri = profile.AvatarFull,
                    RealName = profile.RealName,
                    Url = profile.CustomURL,
                    Username = profile.Headline
                };
                friends.Add(model);
            }

            return friends;
        }
    }
}
