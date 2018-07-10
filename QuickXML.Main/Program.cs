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
        private Tokenizer core = new Tokenizer();
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
        public void TokenizeLargeXML() => core.Tokenize(ref testxml);

        // [Benchmark]
        // public void BuiltInXmlDoc() =>
        //  xmlDoc.LoadXml(testxml);


    }

    class Program
    {
        class MockTokenObs : IXMLParserReceiver
        {

            List<(ParserTokenType, string)> TokenSlice = new List<(ParserTokenType, string)>();

            public void OnCompleted()
            {
                //throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                //throw new NotImplementedException();
            }

            public void OnNext(ParserToken kx)
            {
                TokenSlice.Add((kx.type, XmlBench.testxml.AsSpan().Slice(kx._startIndex, 1 + (kx.endIndex - kx._startIndex)).ToString()));

                //throw new NotImplementedException();
            }
        }
        static void Main(string[] args)
        {
#if !DEBUG
            var summary = BenchmarkRunner.Run<XmlBench>();
#else
            var Xml = new XmlBench();
            Xml.Setup();

            var obs = new MockTokenObs();
            var pipe = new Tokenizer();

            pipe.Subscribe(obs);
            pipe.Tokenize(ref XmlBench.testxml);
#endif
        }
    }
}
