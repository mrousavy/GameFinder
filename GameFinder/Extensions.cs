
namespace GameFinder
{
    public static class Extensions
    {
        public static bool IsValid(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
