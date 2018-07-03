using Jellyfish;

namespace GameFinder.Main
{
    public class MainViewModel : ViewModel
    {
        private int _transitionerIndex;

        public MainViewModel()
        {
            var feed = MessageFeed<TransitionerMoveStruct>.Feed;
            feed.MessageReceived += TransitionerMoveReceived;
        }

        public int TransitionerIndex
        {
            get => _transitionerIndex;
            set => Set(ref _transitionerIndex, value);
        }

        private void TransitionerMoveReceived(TransitionerMoveStruct message)
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