using System;

namespace ConverterToIntLib
{
    /// <summary>
    /// Represents an opportunity to convert string to int
    /// </summary>
    public static class ConverterToInt
    {
        /// <summary>
        /// Convert source string to int.
        /// Overflow is regulated by Build property (Check for arithmetic overflow/underflow) 
        /// </summary>
        /// <param name="sourceString">String to convert.</param>
        /// <returns>Int number of source string.</returns>
        public static int ToInt(this string sourceString)
        {
            if (string.IsNullOrWhiteSpace(sourceString))
            {
                throw new ArgumentNullException(nameof(sourceString));
            }

            var result = 0;

            for (var i = 0; i < sourceString.Length; i++)
            {
                if (!IsFormatCorrect(sourceString[i]))
                {
                    if (sourceString.StartsWith("+"))
                    {
                        return ToInt(sourceString.Remove(0, 1));
                    }

                    if (sourceString.StartsWith("-"))
                    {
                        return ToInt(sourceString.Remove(0, 1)) * -1;
                    }

                    throw new FormatException(nameof(sourceString));
                }

                result += (sourceString[i] - 48) * (int)Math.Pow(10, sourceString.Length - i - 1);
            }

            return result;
        }

        private static bool IsFormatCorrect(char symbol)
        {
            return char.IsNumber(symbol);
        }
    }
}
