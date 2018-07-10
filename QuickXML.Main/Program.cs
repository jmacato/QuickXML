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
    [MemoryDiagnoser]
    public class XmlBench
    {
        private Core core = new Core();
        XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object

        public static string testxml;

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
        // [Benchmark]
        // public void PlainStringRead() => core.PlainStringRead(ref testxml);

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
#if !DEBUG
            var summary = BenchmarkRunner.Run<XmlBench>();
#else
            var Parser = new XmlBench();

            Parser.Setup();

            var Tokens = Parser.ParseLargeXML();
            var TokenSlice = new List<(Core.ParserTokenType, string)>();

            foreach (var kx in Tokens)
            {
                TokenSlice.Add((kx.type, XmlBench.testxml.AsSpan().Slice(kx._startIndex, 1 + (kx.endIndex - kx._startIndex)).ToString()));
            }
#endif
        }
    }
}
