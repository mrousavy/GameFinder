using Jellyfish;

namespace GameFinder.Main
{
    public class MainViewModel : ViewModel
    {
        private int _transitionerIndex;

        public int TransitionerIndex
        {
            get => _transitionerIndex;
            set => Set(ref _transitionerIndex, value);
        }
    }
}
