using System;
using Xunit;
using QuickXML.Main;

namespace QuickXML.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData((char)0x1, false)]
        [InlineData((char)0x9, true)]
        [InlineData((char)0xFFFF, false)]
        public void CheckIfValidChar(ref char target, bool expectedResult)
        {
            Assert.Equal(QuickXML.Main.Parsing.Helpers.IsValidXMLChar(ref target), expectedResult);
        }
    }
}