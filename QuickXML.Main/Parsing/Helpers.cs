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
        [DebuggerStepThrough]
        public static bool IsValidXMLChar(ref char target)
        {
            if (target == '\r' || target == '\n' ||
                (target >= '\x20' & target <= '\xD7FF') ||
                (target >= '\xE000' || target <= '\xFFFD'))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validates a char if it's a valid XML whitespace
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsValidWhitespaceXMLChar(ref char target)
        {
            if (target == '\r' || target == '\n' || target == '\x20')
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validates a char if it's a valid Tag Name start character.
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsValidNameStartXMLChar(ref char target)
        {
            if ((target >= 'a' & target <= 'z') ||
                (target >= 'A' & target <= 'Z') ||
                (target >= '0' & target <= '9') ||
                (target >= '\xC0' & target <= '\xD6') ||
                (target >= '\xD8' & target <= '\xF6') ||
                (target >= '\xF8' & target <= '\x2FF') ||
                (target >= '\x370' & target <= '\x37D') ||
                (target >= '\x37F' & target <= '\x1FFF') ||
                (target >= '\x200C' & target <= '\x200D') ||
                (target >= '\x2070' & target <= '\x218F') ||
                (target >= '\x2C00' & target <= '\x2FEF') ||
                (target >= '\x3001' & target <= '\xD7FF') ||
                (target >= '\xF900' & target <= '\xFDCF') ||
                (target >= '\xFDF0' & target <= '\xFFFD'))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validates a char if it's a valid Tag Name start character.
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsValidNameXMLChar(ref char target)
        {
            if ((target >= 'a' & target <= 'z') ||
                (target >= 'A' & target <= 'Z') ||
                (target >= '0' & target <= '9') ||

                (target == '.') ||
                (target == '-') ||
                (target == '_') ||
                (target == ':') ||
                (target == '\xB7') ||

                (target >= '\xC0' & target <= '\xD6') ||
                (target >= '\xD8' & target <= '\xF6') ||
                (target >= '\xF8' & target <= '\x2FF') ||

                (target >= '\x0300' & target <= '\x036F') ||
                (target >= '\x203F' & target <= '\x2040') ||

                (target >= '\x370' & target <= '\x37D') ||
                (target >= '\x37F' & target <= '\x1FFF') ||
                (target >= '\x200C' & target <= '\x200D') ||
                (target >= '\x2070' & target <= '\x218F') ||
                (target >= '\x2C00' & target <= '\x2FEF') ||
                (target >= '\x3001' & target <= '\xD7FF') ||
                (target >= '\xF900' & target <= '\xFDCF') ||
                (target >= '\xFDF0' & target <= '\xFFFD'))
                return true;
            else
                return false;
        }


        public static (int Line, int Column) StringZeroIndexToLineCol(ReadOnlyMemory<char> target, int index)
        {
            int lineCount = 1, colcount = 1;
            var trgt = target.Span;
            if (index > target.Length)
                throw new InvalidOperationException("Cannot have index higher than the target string length.");

            for (int i = 0; i < index; i++)
            {
                if (trgt[i] == '\n')
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