using System;
using System.Collections.Generic;
using System.IO;

namespace QuickXML
{
    public class Core
    {
        public Core()
        {

        }
        const char _TagCharStart = '<';
        const char _TagCharEnd = '>';
        const char _TagEndIndChar = '/';
        const char _nameAttribDelimitChar = '=';

        private enum ParserState
        {
            StartParse,
            CharStartTagName,
            CharEndTag,
            GetName,
            EndName,
            StartValue,
            EndValue,
            IgnoreUntilCharEndTag,
            InBetweenTag
        }

        public enum ParserTokenType
        {

            TagDeclStart,
            TagName,
            TagEndNodeInd,
            TagDeclEnd,
            Attribute,
            Value,
            EscapedChar,
            TagEmptyInd,
            TagEndIgnoredTag,
        }

        public struct ParserToken
        {
            public ParserTokenType type { get; private set; }
            public int startIndex { get; private set; }
            public int endIndex { get; private set; }

            public ParserToken(ParserTokenType type, int startIndex, int endIndex)
            {
                this.type = type;
                this.startIndex = startIndex;
                this.endIndex = endIndex;
            }
        }

        /// <summary>
        /// Parse an xml string. 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns> 
        public List<ParserToken> ParseString(ReadOnlyMemory<char> _xmlspan)
        {
            List<ParserToken> result = new List<ParserToken>();
            var _stState = ParserState.StartParse;
            int nameStartIndex = 0, nameEndIndex = 0; 
            bool currentlyInTag = false; 
            int savedIndex = 0, savedLength = 0;

            
            for (int i = 0; i < _xmlspan.Length; i++)
            {
                var target = _xmlspan.Span[i];

                if (!Helpers.IsValidXMLChar(ref target))
                    throw new XmlInvalidCharException($"Invalid XML Character at {_xmlspan.Length}");
                loop1:
                switch (_stState)
                {
                    case ParserState.StartParse:

                        switch (target)
                        {
                            case _TagCharStart:

                                _stState = ParserState.CharStartTagName;

                                result.Add(new ParserToken(ParserTokenType.TagDeclStart, i, i));
                                break;

                            case _TagCharEnd:

                                if (currentlyInTag)
                                {
                                    _stState = ParserState.CharEndTag;
                                    goto loop1;
                                }
                                else
                                    throw new XmlParsingFailedException(_xmlspan, i);

                        }

                        break;

                    case ParserState.CharStartTagName:

                        savedIndex = i;

                        // If the name starts with ? or !, ignore it for now.
                        if (target == '?' | target == '!')
                        {
                            _stState = ParserState.IgnoreUntilCharEndTag;
                            break;
                        }
                        else if (target == _TagEndIndChar)
                        {
                            result.Add(new ParserToken(ParserTokenType.TagEndNodeInd, i, i));
                        }
                        else
                        {
                            // Throw if it's not a valid name start char.
                            if (!Helpers.IsValidNameStartXMLChar(ref target))
                                throw new XmlParsingFailedException(_xmlspan, i);
                        }

                        // Save the name index, dont do any string copying for now.
                        _stState = ParserState.GetName;
                        nameStartIndex = i;
                        currentlyInTag = true;
                        break;

                    case ParserState.GetName:


                        // Stop if the next char is a whitespace or the end tag
                        if (Helpers.IsValidWhitespaceXMLChar(ref target))
                        {

                        }
                        else if (target == _TagEndIndChar)
                        {
                            result.Add(new ParserToken(ParserTokenType.TagEmptyInd, i, i));
                            break;
                        }
                        else if (target == _TagCharEnd)
                        {
                            _stState = ParserState.CharEndTag;
                            result.Add(new ParserToken(ParserTokenType.TagName, nameStartIndex, nameEndIndex));
                            goto loop1;
                        }
                        else
                        {
                            // Throw if it's not a valid name char.
                            if (!Helpers.IsValidNameXMLChar(ref target))
                                throw new XmlParsingFailedException(_xmlspan, i);
                        }

                        nameEndIndex = i;
                        break;

                    case ParserState.EndName:

                        _stState = ParserState.StartParse;

                        result.Add(new ParserToken(ParserTokenType.TagName, nameStartIndex, nameEndIndex));

                        break;

                    case ParserState.IgnoreUntilCharEndTag:

                        if (target == _TagCharEnd)
                        {
                            _stState = ParserState.StartParse;
                            result.Add(new ParserToken(ParserTokenType.TagEndIgnoredTag, nameStartIndex, nameEndIndex));
                        }
                        break;

                    case ParserState.CharEndTag:
                        result.Add(new ParserToken(ParserTokenType.TagDeclEnd, i, i));
                        _stState = ParserState.StartParse;
                        currentlyInTag = false;
                        break;

                    default:

                        throw new XmlParsingFailedException(_xmlspan, i);
                }
            }

            return result;

        }
    }
}