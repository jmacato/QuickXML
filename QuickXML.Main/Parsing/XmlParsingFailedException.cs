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
            return ($"QuickXML failed to parse the XML document and stopped at Line {LineCol.Line}, Column {LineCol.Column}.");
        }

        public XmlParsingFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlParsingFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}