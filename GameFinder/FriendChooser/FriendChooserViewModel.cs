using System.Collections.ObjectModel;
using GameFinder.User;
using Jellyfish;

namespace GameFinder.FriendChooser
{
    public class FriendChooserViewModel : ViewModel
    {
        private ObservableCollection<UserViewModel> _friends;

        public ObservableCollection<UserViewModel> Friends
        {
            get => _friends;
            set => Set(ref _friends, value);
        }
    }
}
