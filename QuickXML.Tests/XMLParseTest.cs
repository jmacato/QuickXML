using System;
using Xunit;
using QuickXML;
using Helpers = QuickXML.Helpers;

namespace QuickXML.Tests
{
    public class XMLParseTest
    {
        [Fact]
        public void TestParser1()
        {
          var k = new Core();   
          var test1 = "<!DOCTYPE/>";
          var s = k.ParseString(test1.AsMemory());
        }
    }
}