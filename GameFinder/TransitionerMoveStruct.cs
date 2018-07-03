namespace GameFinder
{
    public enum TransitionerDirection
    {
        Forwards,
        Backwards
    }

    public struct TransitionerMoveStruct
    {
        public TransitionerDirection Direction { get; set; }

        public TransitionerMoveStruct(TransitionerDirection direction)
        {
            Direction = direction;
        }
    }
}