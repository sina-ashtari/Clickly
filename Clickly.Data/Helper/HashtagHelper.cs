
using System.Text.RegularExpressions;

namespace Clickly.Data.Helper
{
    public static class HashtagHelper
    {
        public static List<string> GetHashtags(string postString)
        {
            var hashtagPattern = new Regex(@"#\w+");
            var matches = hashtagPattern
                .Matches(postString).Select(match => match.Value.TrimEnd('.', ',','!','?').ToLower())
                .Distinct().ToList();
            return matches;
        }
    }
}
