using Jellyfish;

namespace GameFinder
{
    public static class Extensions
    {
        public static bool Valid(string str) => !string.IsNullOrWhiteSpace(str);

        public static void MoveForwards()
        {
            var transitionerFeed = MessageFeed<TransitionerMoveStruct>.Feed;
            transitionerFeed.Notify(new TransitionerMoveStruct(TransitionerDirection.Forwards));
        }

        public static void MoveBackwards()
        {
            var transitionerFeed = MessageFeed<TransitionerMoveStruct>.Feed;
            transitionerFeed.Notify(new TransitionerMoveStruct(TransitionerDirection.Backwards));
        }
    }
}