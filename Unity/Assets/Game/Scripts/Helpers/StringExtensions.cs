using System;

namespace Game.Scripts.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Capitalizes the first letter of the string.
        /// </summary>
        /// <param name="s"> String to be capitalized </param>
        /// <returns></returns>
        public static string CapitalizeFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}