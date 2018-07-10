using System;

namespace QuickXML
{
    public interface IXMLParserTransmitter : IObservable<ParserToken>, IDisposable
    {
        
    }
}