
namespace GameFinder
{
    public static class Extensions
    {
        public static bool Valid(string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
