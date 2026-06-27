using System.Collections.Generic;
namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            string lower = input.ToLower();
            List<char> cleanChars = new List<char>();
            foreach (char c in lower)
            {
                if (!char.IsPunctuation(c) && !char.IsWhiteSpace(c)) cleanChars.Add(c);
            }
            if (cleanChars.Count == 0) return false;
            for (int i = 0; i < cleanChars.Count / 2; i++)
            {
                if (cleanChars[i] != cleanChars[cleanChars.Count - 1 - i])
                    return false;
            }
            return true;
        }
    }
}
