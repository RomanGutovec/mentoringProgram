using System;
using NUnit.Framework;

namespace ConverterToIntLib.Tests
{
    [TestFixture]
    public class ConverterToIntLibTests
    {
        [Test]
        public void ConvertToIntTest_InputNullInsteadOfString_ArgumentNullException()
        => Assert.Throws<ArgumentNullException>(() =>
        {
            string sourceString = null;
            sourceString.ToInt();
        });

        [Test]
        public void ConvertToIntTest_InputEmptyString_ArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => string.Empty.ToInt());

        [Test]
        public void ConvertToIntTest_InputWhiteSpaceString_ArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => " ".ToInt());

        [TestCase("#123456789")]
        [TestCase("-123456789-")]
        [TestCase("-789-7895")]
        [TestCase("+123456+789")]
        [TestCase("+515+")]
        [TestCase("+1258774a")]
        [TestCase("somestring")]
        [TestCase("-somestring")]
        public void ConvertToIntTest_InputStringWithIncorrectData_FormatException(string sourceString)
            => Assert.Throws<FormatException>(() => sourceString.ToInt());

        [TestCase(int.MaxValue, 15)]
        [TestCase(int.MaxValue, 1)]
        [TestCase(int.MinValue, -1)]
        [TestCase(int.MinValue, -100)]
        public void ConvertToIntTest_InputStringWithValueMoreThanMaxValue_OverflowException(int value, int valueToOverload)
            => Assert.Throws<OverflowException>(() => (value + valueToOverload.ToString()).ToInt());


        [TestCase("123487645", 123487645)]
        [TestCase("+1896", 1896)]
        [TestCase("+123328", 123328)]
        [TestCase("-85543328", -85543328)]
        [TestCase("-855", -855)]
        public void ConvertToIntTest_InputStringWithValueMoreThanMaxValue_OverflowException(string sourceString, int expectedResult)
            => Assert.AreEqual(expectedResult, sourceString.ToInt());
    }
}