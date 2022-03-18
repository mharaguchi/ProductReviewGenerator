namespace ReviewGenerator.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEndOfSentence(this string word)
        {
            return word == "." || word == "!";
        }
    }
}
