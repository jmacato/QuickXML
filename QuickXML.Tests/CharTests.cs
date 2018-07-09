using System;
using Xunit;
using QuickXML.Main;
using Helpers = QuickXML.Helpers;

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
            Assert.Equal(Helpers.IsValidXMLChar(ref target), expectedResult);
        }

        [Theory]
        [InlineData('A', false)]
        [InlineData('1', false)]
        [InlineData(' ', true)]
        [InlineData('\t', true)]
        [InlineData('\n', true)]
        public void Check_If_Valid_Whitespace_Char(ref char target, bool expectedResult)
        {
            Assert.Equal(Helpers.IsValidWhitespaceXMLChar(ref target), expectedResult);
        }

        [Theory]
        [InlineData('A', true)]
        [InlineData('a', true)]
        [InlineData('1', false)]
        [InlineData(' ', false)]
        [InlineData('.', false)]
        [InlineData('\t', false)]
        [InlineData('\n', false)]
        public void Check_If_Valid_NameStart_Char(ref char target, bool expectedResult)
        {
            Assert.Equal(Helpers.IsValidNameStartXMLChar(ref target), expectedResult);
        }
        
        [Theory]
        [InlineData('A', true)]
        [InlineData('a', true)]
        [InlineData('1', true)]
        [InlineData(' ', false)]
        [InlineData('.', true)]
        [InlineData('\t', false)]
        [InlineData('\n', false)]
        public void Check_If_Valid_Name_Char(ref char target, bool expectedResult)
        {
            Assert.Equal(Helpers.IsValidNameXMLChar(ref target), expectedResult);
        }
    }
}