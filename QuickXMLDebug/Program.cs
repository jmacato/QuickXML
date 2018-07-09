using System;
using QuickXML;

namespace QuickXMLDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var k = new Core();   
            var test1 = "<?xml/><fody></fody><sdfs></sadfsdf    ><俄:语></俄语   >";
            var o = k.ParseString(test1.AsMemory());
        }
    }
}
