using System;
using Xunit;
using QuickXML.Main;

namespace QuickXML.Tests
{
    public class CharTests
    {
        [Theory]
        [InlineData((char)0x0, false)]
        [InlineData((char)0x1, false)]
        [InlineData((char)0x9, true)]
        [InlineData((char)0xFFFF, false)]
        public void Check_If_ValidChar(ref char target, bool expectedResult)
        {
            Assert.Equal(QuickXML.Main.Parsing.Helpers.IsValidXMLChar(ref target), expectedResult);
        }

        [Theory]
        [InlineData('A', false)]
        [InlineData('1', false)]
        [InlineData(' ', true)]
        [InlineData('\t', true)]
        [InlineData('\n', true)]
        public void Check_If_Valid_Whitespace_Char(ref char target, bool expectedResult)
        {
            Assert.Equal(QuickXML.Main.Parsing.Helpers.IsWhitespaceXMLChar(ref target), expectedResult);
        }
    }
}