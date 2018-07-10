using System.Diagnostics;

namespace QuickXML
{
    [DebuggerStepThrough]
    public struct ParserToken
    {
        public readonly ParserTokenType type;
        public readonly int _startIndex;
        public readonly int endIndex;

        [DebuggerStepThrough]
        public ParserToken(ParserTokenType type, int startIndex, int endIndex)
        {
            this.type = type;
            this._startIndex = startIndex;
            this.endIndex = endIndex;
        }
    }
}