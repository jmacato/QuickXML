using System;
using System.Diagnostics;

namespace QuickXML
{
    public static partial class Helpers
    {
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