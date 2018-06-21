using System;

namespace QuickXML.Main.Parsing
{
    public class Helpers
    {
        readonly static (char startRange, char endRange)[] validCharRanges = new(char, char)[]
        {
            ('\x9','\xA'),
            ('\xD','\xD'),
            ('\x20','\xD7FF'),
            ('\xE000','\xFFFD'),        
            //(0x10000,0x10FFFF),     // This might not work with 16-bit wide chars in .NET
                                      // But i'll keep this for reference's sake.
        };

        readonly static (char startRange, char endRange)[] validWhitespaceRanges = new(char, char)[]
        {
            ('\x9','\xA'),
            ('\xD','\xD'),
            ('\x20', '\x20')
        };

        /// <summary>
        /// Validates a char against XML spec's valid
        /// character definition.
        /// </summary>
        public static bool IsValidXMLChar(ref char target)
        {
            var range = (ReadOnlySpan<(char, char)>)validCharRanges;

            return validateCharRange(ref range, ref target);
        }

        /// <summary>
        /// Validates a char if it's a XML known whitespace
        /// </summary>
        public static bool IsWhitespaceXMLChar(ref char target)
        {
            var range = (ReadOnlySpan<(char, char)>)validWhitespaceRanges;

            return validateCharRange(ref range, ref target);
        }

        private static bool validateCharRange(ref ReadOnlySpan<(char start, char end)> charRanges, ref char target)
        {
            for (int i = 0; i < charRanges.Length; i++)
            {
                var validRange = charRanges[i];
                if (target >= validRange.start & target <= validRange.end)
                    break;
                else
                    return false;
            }
            return true;
        }
    }
}