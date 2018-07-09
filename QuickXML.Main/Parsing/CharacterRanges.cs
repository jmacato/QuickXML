namespace QuickXML
{
    public static partial class Helpers
    {
        readonly static (char, char)[] valid_Char_Ranges = new(char, char)[]
        {
            ('\x9','\xA'),
            ('\xD','\xD'),
            ('\x20','\xD7FF'),
            ('\xE000','\xFFFD'),        
            //(0x10000,0x10FFFF),     // Keeping this for reference purposes.
        };

        readonly static (char, char)[] valid_Whitespace_Ranges = new(char, char)[]
        {
            ('\x9','\xA'),
            ('\xD','\xD'),
            ('\x20', '\x20')
        };

        readonly static (char, char)[] valid_NameStartChar_Ranges = new(char, char)[]
        {
            (':',':'),
            ('A','Z'),
            ('_', '_'),
            ('a', 'z'),
            ('\xC0','\xD6'),
            ('\xD8','\xF6'),
            ('\xF8','\x2FF'),
            ('\x370','\x37D'),
            ('\x37F','\x1FFF'),
            ('\x200C','\x200D'),
            ('\x2070','\x218F'),
            ('\x2C00','\x2FEF'),
            ('\x3001','\xD7FF'),
            ('\xF900','\xFDCF'),
            ('\xFDF0','\xFFFD')
            //('\x10000','\xEFFFF') // Keeping this for reference purposes.
        };

        readonly static (char, char)[] valid_NameChar_Ranges = new(char, char)[]
        {
            (':',':'),
            ('A','Z'),
            ('a', 'z'),
            ('_', '_'),

            ('-', '-'),
            ('.', '.'),
            ('0', '9'),
            ('\xB7', '\xB7'),
            ('\x0300', '\x036F'),
            ('\x203F', '\x2040'),

            ('\xC0','\xD6'),
            ('\xD8','\xF6'),
            ('\xF8','\x2FF'),
            ('\x370','\x37D'),
            ('\x37F','\x1FFF'),
            ('\x200C','\x200D'),
            ('\x2070','\x218F'),
            ('\x2C00','\x2FEF'),
            ('\x3001','\xD7FF'),
            ('\xF900','\xFDCF'),
            ('\xFDF0','\xFFFD')
            //('\x10000','\xEFFFF') // Keeping this for reference purposes.
        };
    }
}