using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;

namespace QuickXML
{
    public class XmlBench
    {
        private Core core = new Core();
        XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object

        static string testxml;

        ReadOnlyMemory<char> xml = testxml.AsMemory();

        [GlobalSetup]
        public void Setup()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "QuickXML.Main.test.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    testxml = reader.ReadToEnd();
                }
            }
        }
        [Benchmark]
        public void PlainStringRead() => core.PlainStringRead(ref testxml);
        [Benchmark]
        public List<Core.ParserToken> ParseLargeXML() => core.ParseString(ref testxml);

        [Benchmark]
        public void BuiltInXmlDoc() =>
         xmlDoc.LoadXml(testxml);


    }

    class Program
    {

        static void Main(string[] args)
        {
           var summary = BenchmarkRunner.Run<XmlBench>();
            //    var k = new XmlBench();
            //    k.Setup();
            //    var res = k.ParseLargeXML();


            // var assembly = Assembly.GetExecutingAssembly();
            // var resourceName = "QuickXML.Main.test.xml";

            // using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            // {
            //     using (StreamReader reader = new StreamReader(stream))
            //     {
            //         var testxml = reader.ReadToEnd();
            //         XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
            //         xmlDoc.LoadXml(testxml);
                    
            //     }
            // }
        }
    }
}
