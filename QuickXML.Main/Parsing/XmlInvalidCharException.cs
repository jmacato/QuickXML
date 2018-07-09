using System;
using System.Runtime.Serialization;

namespace QuickXML
{
    [Serializable]
    internal class XmlInvalidCharException : Exception
    {
        public XmlInvalidCharException()
        {
        }

        public XmlInvalidCharException(string message) : base(message)
        {
        }

        public XmlInvalidCharException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlInvalidCharException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}