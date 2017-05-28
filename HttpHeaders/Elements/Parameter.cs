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
            new OrExpression("value") {
                new QuotedString("quotedvalue"),
                new Token("tokenvalue")
            }
            
        };


        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Name + "=" + Value;
        }

        public static Parameter Create(ParseNode node)
        {
            var valueNode = node["tokenvalue"] ?? node["quotedvalue"];
            return new Parameter()
            {
                Name = node["name"].Text,
                Value = valueNode.Text 
            };
        }
    }
}