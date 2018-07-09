using System;
using System.Diagnostics;

namespace QuickXML
{
    public static partial class Helpers
    {
        /// <summary>
        /// Validates a char against XML spec's valid
        /// character definition.
        /// </summary>
        public static bool IsValidXMLChar(ref char target)
            => validateCharRange(valid_Char_Ranges, ref target);

        /// <summary>
        /// Validates a char if it's a valid XML whitespace
        /// </summary>
        public static bool IsValidWhitespaceXMLChar(ref char target)
            => validateCharRange(valid_Whitespace_Ranges, ref target);

        /// <summary>
        /// Validates a char if it's a valid Tag Name start character.
        /// </summary>
        public static bool IsValidNameStartXMLChar(ref char target)
            => validateCharRange(valid_NameStartChar_Ranges, ref target);

        /// <summary>
        /// Validates a char if it's a valid Tag Name start character.
        /// </summary>
        public static bool IsValidNameXMLChar(ref char target)
            => validateCharRange(valid_NameChar_Ranges, ref target);

        /// <summary>
        /// Core validation function
        /// </summary>
        [DebuggerStepThrough]
        private static bool validateCharRange(ReadOnlySpan<(char start, char end)> charRanges, ref char target)
        {
            foreach (var validRange in charRanges)
                if (target >= validRange.start & target <= validRange.end)
                    return true;
            return false;
        }
 
        public static (int Line, int Column) StringZeroIndexToLineCol(ReadOnlyMemory<char> target, int index)
        {
            int lineCount = 1, colcount = 1;

            if (index > target.Length)
                throw new InvalidOperationException("Cannot have index higher than the target string length.");

            for (int i = 0; i < index; i++)
            {
                if (target.Span[i] == '\n')
                {
                    lineCount++;
                    colcount = 1;
                }
                else
                {
                    colcount++;
                }
            }

            return (lineCount, colcount);
        }
    }
}