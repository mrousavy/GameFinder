using Jellyfish;
using Jellyfish.Feeds;

namespace GameFinder.Main
{
    public class MainViewModel : ViewModel, INode<TransitionerMoveStruct>
    {
        private int _transitionerIndex;

        public MainViewModel()
        {
            var feed = Feed<TransitionerMoveStruct>.Instance;
            feed.RegisterNode(this);
            DiscordHelper.Initialize();
        }

        public int TransitionerIndex
        {
            get => _transitionerIndex;
            set => Set(ref _transitionerIndex, value);
        }

        public void MessageReceived(TransitionerMoveStruct message)
        {
            switch (message.Direction)
            {
                case TransitionerDirection.Backwards:
                    TransitionerIndex--;
                    break;
                case TransitionerDirection.Forwards:
                    TransitionerIndex++;
                    break;
            }
        }
    }
}