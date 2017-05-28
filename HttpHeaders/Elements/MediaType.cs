using Tavis.Parser;

namespace Tavis.Headers.Elements
{
    public class MediaType
    {

        public static IExpression Syntax = new Expression("mediatype")
        {
            new Token("type"),
            new Literal("/"),
            new Token("subtype"),

        };
        public string Type { get; set; }
        public string SubType { get; set; }

        public override string ToString()
        {
            return Type + "/" + SubType;
        }

        public static MediaType Create(ParseNode node)
        {
            return new MediaType()
            {
                Type = node["type"].Text,
                SubType = node["subtype"].Text
            };
        }
    }
}