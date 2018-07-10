using System;
using System.Runtime.Serialization;
using static QuickXML.Helpers;

namespace QuickXML
{
    [Serializable]
    internal class XmlParsingFailedException : Exception
    {
        public XmlParsingFailedException()
        {
        }

        public XmlParsingFailedException(string message) : base(message)
        {
        }

        public XmlParsingFailedException(ReadOnlyMemory<char> xmlspan, int i) : base(GenerateMessage(xmlspan, i))
        {
        }

        private static string GenerateMessage(ReadOnlyMemory<char> xmlspan, int i)
        {
            var LineCol = StringZeroIndexToLineCol(xmlspan, i);

            var window_min = i > 0 && i < xmlspan.Length && (i - 10) > 0 ? i - 10 : 0;
            var window_max = i > 0 && i < xmlspan.Length && (i + 10) < xmlspan.Length ? i + 10 : xmlspan.Length;


            return ($"QuickXML failed to parse the XML document and stopped at Line {LineCol.Line}, Column {LineCol.Column}.\n Snippet \"{xmlspan.Slice(window_min, 1 + (window_max - window_min))}\"");
        }

        public XmlParsingFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlParsingFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}