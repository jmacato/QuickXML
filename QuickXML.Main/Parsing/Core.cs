using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            InBetweenTag,
            ExpectAttributesOrEndCharTag
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
            TextContent,
            AttributeName,
        }

        [DebuggerStepThrough]
        public class ParserToken
        {
            public ParserTokenType type;
            public int startIndex;
            public int endIndex;
            //public ReadOnlyMemory<char> text;

            [DebuggerStepThrough]
            public ParserToken(ReadOnlyMemory<char> _xmlspan, ParserTokenType type, int startIndex, int endIndex)
            {
                this.type = type;
                this.startIndex = startIndex;
                this.endIndex = endIndex;
                // this.text = _xmlspan.Slice(startIndex, (endIndex - startIndex) + 1);
            }
        }

        /// <summary>
        /// Parse an xml string. 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns> 
        public List<ParserToken> ParseString(ref string xmlspan)
        {
            var _xmlspan = xmlspan.AsMemory();
            //List<ParserToken> result = new List<ParserToken>();

            var _stState = ParserState.StartParse;

            int nameStartIndex = 0,
                nameEndIndex = 0,
                textStartIndex = 0,
                textEndIndex = 0;

            bool currentlyInTag = false,
                 nextNamesAreAttributes = false,
                 getAttributeName = false,
                 bool23 = false,
                 startTextContent = false
                 ;

            int savedIndex = 0,
                savedLength = 0;


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

                                if (!currentlyInTag)
                                {
                                    if (textStartIndex < textEndIndex)
                                    {
                                        //result.Add(new ParserToken(_xmlspan, ParserTokenType.TextContent, textStartIndex, textEndIndex));
                                    }
                                }

                                _stState = ParserState.CharStartTagName;

                                // result.Add(new ParserToken(_xmlspan, ParserTokenType.TagDeclStart, i, i));
                                break;

                            case _TagCharEnd:
                                if (currentlyInTag)
                                {
                                    _stState = ParserState.CharEndTag;
                                    goto loop1;
                                }
                                else
                                    throw new XmlParsingFailedException(_xmlspan, i);

                            default:
                                if (Helpers.IsValidWhitespaceXMLChar(ref target) | Helpers.IsValidXMLChar(ref target))
                                {
                                    if (!currentlyInTag)
                                    {
                                        if (startTextContent)
                                        {
                                            textStartIndex = i;
                                            startTextContent = false;
                                        }
                                        textEndIndex = i;
                                    }
                                    break;
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
                            //result.Add(new ParserToken(_xmlspan, ParserTokenType.TagEndNodeInd, i, i));
                            savedIndex++;
                        }
                        else
                        {
                            // Throw if it's not a valid name start char.
                            if (!Helpers.IsValidNameStartXMLChar(ref target))
                                throw new XmlParsingFailedException(_xmlspan, i);
                        }

                        // Save the name index, dont do any string copying for now.
                        _stState = ParserState.GetName;
                        nameStartIndex = savedIndex;
                        nameEndIndex = nameStartIndex;
                        currentlyInTag = true;
                        break;

                    case ParserState.GetName:

                        if (Helpers.IsValidNameXMLChar(ref target))
                        {
                            nameEndIndex = i;
                        }

                        // Stop if the next char is a whitespace or the end tag
                        if (Helpers.IsValidWhitespaceXMLChar(ref target))
                        {
                            if (!nextNamesAreAttributes)
                            {
                                // result.Add(new ParserToken(_xmlspan, ParserTokenType.TagName, nameStartIndex, nameEndIndex));
                                nextNamesAreAttributes = true;
                            }
                            else if (getAttributeName)
                            {
                                //  result.Add(new ParserToken(_xmlspan, ParserTokenType.AttributeName, nameStartIndex, nameEndIndex));
                                getAttributeName = false;
                            }


                            break;
                        }
                        else if (target == _TagEndIndChar)
                        {
                            //  result.Add(new ParserToken(_xmlspan, ParserTokenType.TagEmptyInd, i, i));
                            nextNamesAreAttributes = false;

                            break;
                        }
                        else if (target == _TagCharEnd)
                        {
                            _stState = ParserState.CharEndTag;
                            //  result.Add(new ParserToken(_xmlspan, ParserTokenType.TagName, nameStartIndex, nameEndIndex));
                            nextNamesAreAttributes = false;
                            goto loop1;
                        }

                        else
                        {
                            // Throw if it's not a valid name char.
                            if (!Helpers.IsValidNameXMLChar(ref target))
                                throw new XmlParsingFailedException(_xmlspan, i);

                            if (nextNamesAreAttributes && !getAttributeName)
                            {
                                nameStartIndex = i;
                                getAttributeName = true;
                            }

                        }

                        break;

                    case ParserState.EndName:

                        _stState = ParserState.StartParse;

                        //  result.Add(new ParserToken(_xmlspan, ParserTokenType.TagName, nameStartIndex, nameEndIndex));

                        break;

                    case ParserState.IgnoreUntilCharEndTag:

                        if (target == _TagCharEnd)
                        {
                            _stState = ParserState.StartParse;
                            // result.Add(new ParserToken(_xmlspan, ParserTokenType.TagEndIgnoredTag, nameStartIndex, nameEndIndex));
                        }
                        break;

                    case ParserState.CharEndTag:
                        // result.Add(new ParserToken(_xmlspan, ParserTokenType.TagDeclEnd, i, i));
                        _stState = ParserState.StartParse;
                        currentlyInTag = false;
                        startTextContent = true;
                        break;

                    default:

                        throw new XmlParsingFailedException(_xmlspan, i);
                }
            }

            //return result;
            return null;

        }


        public void PlainStringRead(ref string xmlspan)
        {
            var _xmlspan = xmlspan.AsMemory();
            List<ParserToken> result = new List<ParserToken>();

            var _stState = ParserState.StartParse;

            int nameStartIndex = 0,
                nameEndIndex = 0,
                textStartIndex = 0,
                textEndIndex = 0;

            bool currentlyInTag = false,
                 nextNamesAreAttributes = false,
                 getAttributeName = false,
                 bool23 = false,
                 startTextContent = false
                 ;

            int savedIndex = 0,
                savedLength = 0;


            for (int i = 0; i < _xmlspan.Length; i++)
            {
                var target = _xmlspan.Span[i];
            }

        }
    }
}