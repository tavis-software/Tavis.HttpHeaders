using Headers;
using Tavis.Parser;

namespace Tavis.Headers.Elements
{
    public class Parameter
    {
        public static IExpression Syntax = new Expression("parameter")
        {
            new Token("name"),
            new Ows(),
            new Literal("="),
            new Ows(),
            new Token("value")
        };


        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Name + "=" + Value;
        }

        public static Parameter Create(ParseNode node)
        {
            return new Parameter()
            {
                Name = node["name"].Text,
                Value= node["value"].Text
            };
        }
    }
}