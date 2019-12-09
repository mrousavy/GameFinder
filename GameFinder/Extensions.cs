using Jellyfish;
using Jellyfish.Feeds;

namespace GameFinder
{
    public static class Extensions
    {
        public static bool Valid(string str) => !string.IsNullOrWhiteSpace(str);

        public static void MoveForwards()
        {
            var transitionerFeed = Feed<TransitionerMoveStruct>.Instance;
            transitionerFeed.Notify(new TransitionerMoveStruct(TransitionerDirection.Forwards));
        }

        public static void MoveBackwards()
        {
            var transitionerFeed = Feed<TransitionerMoveStruct>.Instance;
            transitionerFeed.Notify(new TransitionerMoveStruct(TransitionerDirection.Backwards));
        }
    }
}