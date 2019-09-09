using System;

namespace FirstCharGetter
{
    public static class ExtensionString
    {
        /// <summary>
        /// Represents an opportunity to get first char of a string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when source string is null or empty.</exception>
        /// <param name="sourceString">Source string to get first char</param>
        /// <returns>First char of string</returns>
        public static char  FirstChar(this string sourceString)
        {
            if (string.IsNullOrEmpty(sourceString))
            {
                throw new ArgumentNullException($"String {nameof(sourceString)} is empty.");
            }

            return sourceString[0];
        }
    }
}
