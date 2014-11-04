using System;
using Tavis.Parser;

namespace Tavis.Headers
{
    public class Qvalue 
    {
        public decimal Weight { get; set; }

        public static IExpression Syntax =
            new OrExpression("o1")
            {
                new Expression("w0")
                {
                    new Literal("0"),
                    new OptionalExpression("dp0") {new Literal("."), new BasicRule("dec0", BasicRule.Digit, 3)}
                },
                new Expression("w1")
                {
                    new Literal("1"),
                    new OptionalExpression("dp1") {new Literal("."), new BasicRule("dec1", (c) => c == '0', 3)}
                }
            };


    
        public static Qvalue Create(ParseNode node)
        {
            if (node.Expression.Identifier=="w0")
            {
                if (node.ChildNode("dp0").Present)
                {
                    return new Qvalue() { Weight = Convert.ToDecimal("0." + node.ChildNode("dp0").ChildNode("dec0").Text) };
                }
                else
                {
                    return new Qvalue() {Weight = 0};
                }
            }

            if (node.Expression.Identifier=="w1")
            {
                return new Qvalue() { Weight = 1 };
            }
            return new Qvalue() { Weight = -1 };
        }
    }
}