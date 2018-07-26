using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace QuickXML
{
    public class Tokenizer : IXMLParserTransmitter
    {
        const char _TagCharStart = '<';
        const char _TagCharEnd = '>';
        const char _TagEndIndChar = '/';
        const char _nameAttribDelimitChar = '=';
        private IObserver<ParserToken> Receiver;

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

        void PumpToken(ParserToken p)
        {
            Receiver?.OnNext(p);
        }

        /// <summary>
        /// Parse an xml string. 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns> 
        public void Tokenize(ref string xmlspan, bool ignoreErrors = false)
        {
            var _xmlspan = xmlspan.AsSpan();
            //List<ParserToken> result = new List<ParserToken>();

            var _stState = ParserState.StartParse;

            int nameStartIndex = 0,
                nameEndIndex = 0,
                textStartIndex = 0,
                textEndIndex = 0;

            bool currentlyInTag = false,
                 nextNamesAreAttributes = false,
                 getAttributeName = false,
                 startTextContent = false;

            int savedIndex = 0;



            for (int i = 0; i < _xmlspan.Length; i++)
            {
                bool SeekAfterError(Exception e)
                {
                    if (ignoreErrors)
                    {
                        //Reset states and try again.
                        _stState = ParserState.StartParse;

                        currentlyInTag = false;
                        nextNamesAreAttributes = false;
                        getAttributeName = false;
                        startTextContent = false; 

                        return true;
                    }
                    else
                    {
                        Receiver?.OnError(e);
                        return false;
                    };
                }

                var target = _xmlspan[i];

                if (!Helpers.IsValidXMLChar(ref target))
                {
                    Receiver?.OnError(new XmlInvalidCharException($"Invalid XML Character at {_xmlspan.Length}"));
                    return;
                }


            loop1:
                switch (_stState)
                {
                    case ParserState.StartParse:

                        switch (target)
                        {
                            case _TagCharStart:

                                if (!currentlyInTag && textStartIndex < textEndIndex)
                                {
                                    PumpToken(new ParserToken(ParserTokenType.TextContent, textStartIndex, textEndIndex));
                                }

                                _stState = ParserState.CharStartTagName;

                                PumpToken(new ParserToken(ParserTokenType.TagDeclStart, i, i));
                                break;

                            case _TagCharEnd:
                                if (currentlyInTag)
                                {
                                    _stState = ParserState.CharEndTag;
                                    goto loop1;
                                }
                                else
                                 
                                 if (SeekAfterError(new XmlParsingFailedException(xmlspan.AsMemory(), i))) continue; else return;

                            default:
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
                            PumpToken(new ParserToken(ParserTokenType.TagEndNodeInd, i, i));
                            savedIndex++;
                        }
                        else
                        {
                            // Throw if it's not a valid name start char.
                            if (!Helpers.IsValidNameStartXMLChar(ref target))
                                if (SeekAfterError(new XmlParsingFailedException(xmlspan.AsMemory(), i))) continue; else return;

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
                                PumpToken(new ParserToken(ParserTokenType.TagName, nameStartIndex, nameEndIndex));
                                nextNamesAreAttributes = true;
                            }
                            else if (getAttributeName)
                            {
                                PumpToken(new ParserToken(ParserTokenType.Attribute, nameStartIndex, nameEndIndex));
                                getAttributeName = false;
                            }

                            break;
                        }
                        else if (target == _TagEndIndChar)
                        {
                            PumpToken(new ParserToken(ParserTokenType.TagEmptyInd, i, i));
                            nextNamesAreAttributes = false;

                            break;
                        }
                        else if (target == _TagCharEnd)
                        {
                            _stState = ParserState.CharEndTag;

                            if (nextNamesAreAttributes)
                                PumpToken(new ParserToken(ParserTokenType.Attribute, nameStartIndex, nameEndIndex));
                            else
                                PumpToken(new ParserToken(ParserTokenType.TagName, nameStartIndex, nameEndIndex));

                            nextNamesAreAttributes = false;
                            goto loop1;
                        }

                        else
                        {
                            // Throw if it's not a valid name char.
                            if (!Helpers.IsValidNameXMLChar(ref target))
                                if (SeekAfterError(new XmlParsingFailedException(xmlspan.AsMemory(), i))) continue; else return;

                            if (nextNamesAreAttributes && !getAttributeName)
                            {
                                nameStartIndex = i;
                                getAttributeName = true;
                            }
                        }

                        break;

                    case ParserState.EndName:

                        _stState = ParserState.StartParse;

                        PumpToken(new ParserToken(ParserTokenType.TagName, nameStartIndex, nameEndIndex));

                        break;

                    case ParserState.IgnoreUntilCharEndTag:

                        if (target == _TagCharEnd)
                        {
                            _stState = ParserState.StartParse;
                            PumpToken(new ParserToken(ParserTokenType.TagEndIgnoredTag, nameStartIndex, nameEndIndex));
                        }
                        break;

                    case ParserState.CharEndTag:
                        PumpToken(new ParserToken(ParserTokenType.TagDeclEnd, i, i));
                        _stState = ParserState.StartParse;
                        currentlyInTag = false;
                        startTextContent = true;
                        break;

                    default:

                        if (SeekAfterError(new XmlParsingFailedException(xmlspan.AsMemory(), i))) continue; else return;
                }
            }

            Receiver?.OnCompleted();
        }

        public void PlainStringRead(ref string xmlspan)
        {
            var _xmlspan = xmlspan.AsSpan();
            for (int i = 0; i < _xmlspan.Length; i++)
            {
                var target = _xmlspan[i];
            }
        }

        public IDisposable Subscribe(IObserver<ParserToken> observer)
        {
            if (Receiver == null)
                Receiver = observer;
            else
                throw new InvalidOperationException($"{this.GetType()} can only accept a subscriber once.");

            return this;
        }

        public void Dispose()
        {
            Receiver = null;
        }
    }
}