using System;
using System.Collections.Generic;

namespace QuickXML.Main.XML
{
    public struct XmlNode
    {
        ReadOnlyMemory<char> RawText { get; set; }
        public string Name { get; set; }
        public IList<XmlAttributes> Attributes { get; set; }
        public IList<XmlNode> Children { get; set; }
    }
}