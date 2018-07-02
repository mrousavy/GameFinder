using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameFinder.UserSmall;
using Jellyfish;

namespace GameFinder.FriendChooser
{
    public class FriendChooserViewModel : ViewModel
    {
        private ObservableCollection<UserSmallViewModel> _allFriends;

        public ObservableCollection<UserSmallViewModel> AllFriends
        {
            get => _allFriends;
            set => Set(ref _allFriends, value);
        }

        private ObservableCollection<UserSmallViewModel> _chosenFriends;

        public ObservableCollection<UserSmallViewModel> ChosenFriends
        {
            get => _chosenFriends;
            set => Set(ref _chosenFriends, value);
        }

        public FriendChooserViewModel(IEnumerable<UserSmallViewModel> friends)
        {
            AllFriends = new ObservableCollection<UserSmallViewModel>(friends);
            ChosenFriends = new ObservableCollection<UserSmallViewModel>();
        }

        public FriendChooserViewModel()
        {
            AllFriends = new ObservableCollection<UserSmallViewModel>();
            ChosenFriends = new ObservableCollection<UserSmallViewModel>();
        }
    }
}
