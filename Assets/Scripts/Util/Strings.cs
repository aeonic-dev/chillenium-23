using System.Text.RegularExpressions;

namespace Util {
    public static class Strings {
        public static string Prettify(this string s) {
            return Regex.Replace(s, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
        }
    }
}