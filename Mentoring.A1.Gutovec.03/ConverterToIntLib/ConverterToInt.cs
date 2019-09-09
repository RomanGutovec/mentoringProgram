using System;

namespace ConverterToIntLib
{
    /// <summary>
    /// Represents an opportunity to convert string to int
    /// </summary>
    public class ConverterToInt
    {
        /// <summary>
        /// Convert source string to int.
        /// Overflow is regulated by Build property (Check for arithmetic overflow/underflow) 
        /// </summary>
        /// <param name="sourceString">String to convert.</param>
        /// <returns>Int number of source string.</returns>
        public int ConvertToInt(string sourceString)
        {
            if (string.IsNullOrEmpty(sourceString) || string.IsNullOrWhiteSpace(sourceString))
            {
                throw new ArgumentNullException($"String {nameof(sourceString)} has null value.");
            }

            int result = 0;

            for (int i = 0; i < sourceString.Length; i++)
            {
                if (!IsFormatCorrect(sourceString[i]))
                {
                    if (sourceString.StartsWith("+"))
                    {
                        return ConvertToInt(sourceString.Remove(0, 1));
                    }

                    if (sourceString.StartsWith("-"))
                    {
                        return ConvertToInt(sourceString.Remove(0, 1)) * (-1);
                    }

                    throw new FormatException($"String {nameof(sourceString)} has incorrect format.");
                }

                result += (sourceString[i] - 48) * (int)Math.Pow(10, (sourceString.Length - i - 1));
            }

            return result;
        }

        private bool IsFormatCorrect(char symbol)
        {
            return char.IsNumber(symbol);
        }
    }
}
