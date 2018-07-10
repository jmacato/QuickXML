namespace QuickXML
{
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
}