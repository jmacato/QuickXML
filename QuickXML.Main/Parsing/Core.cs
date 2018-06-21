using System;
using System.Collections.Generic;
using System.IO;

namespace QuickXML.Main.Parsing
{
    public class Core
    {

        const char _TagCharStart = '<';
        const char _TagCharEnd = '>';
        const char _endTagIndChar = '/';
        const char _nameAttribDelimitChar = '=';

        private enum ParserStateMachineState
        {
            StartParse,
            StartTag,
            EndTag,
            StartName,
            EndName,
            StartValue,
            EndValue
        }

        public enum ParserTokenType
        {
            StartTag,
            EndTag,
            Attribute,
            Value,
            EscapedChar
        }

        public struct ParserToken
        {
            public ParserTokenType type { get; private set; }
            public int index { get; private set; }
            public int length { get; private set; }

            public ParserToken(ParserTokenType type, int index, int length)
            {
                this.type = type;
                this.index = index;
                this.length = length;
            }
        }

        /// <summary>
        /// Parse an xml string. 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public IEnumerator<ParserToken> ParseString(ref string xmlString)
            => InternalParse(xmlString.AsSpan());

        IEnumerator<ParserToken> InternalParse(ReadOnlySpan<char> _xmlspan)
        {
            var _stState = ParserStateMachineState.StartParse;

            for (int i = 0; i < _xmlspan.Length; i++)
            {
                var target = _xmlspan[i];
                int savedIndex, savedLength;
                bool isEndTag = false;


                if (!Helpers.IsValidXMLChar(ref target))
                    throw new XmlInvalidCharException($"Invalid XML Character at {_xmlspan.Length}");

                switch (target)
                {
                    case _TagCharStart:
                        savedIndex = i;

                        break;

                    case _endTagIndChar:


                        break;

                    case _TagCharEnd:

                        break;

                    default:

                        break;
                }
            }
        }
    }
}