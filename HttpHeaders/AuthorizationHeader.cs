using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.Headers.Elements;
using Tavis.Parser;

namespace Headers
{
    public class AuthorizationHeaderValue
    {
        //credentials = auth-scheme [ 1*SP ( token68 / #auth-param ) ]

        private static readonly IExpression _Syntax = new RootExpression("authorization")
            {
                new Token("scheme"),
                
                new OptionalExpression("parameter") {
                    new Literal(" "),
                    new OrExpression("") {
                        new CommaList("parameterpairs", Tavis.Headers.Elements.Parameter.Syntax),
                        new Expression("token68") {
                            new BasicRule("token68value",BasicRule.Token68Char),
                            new BasicRule("token68delimiter",c => c == '=',2)
                        }
                    }
                }
            };

        public string Scheme { get; set; }
        public List<Parameter> Parameter { get; set; }
        public string Token { get; set; }
        public List<string> Errors {get;set;}

        public static AuthorizationHeaderValue Parse(string rawHeaderValue)
        {
            var node = _Syntax.Consume(new Inputdata(rawHeaderValue));
           
            var headerValue = new AuthorizationHeaderValue
            {
                Errors = node.GetErrors()
            };

            foreach (var parseNode in node.ChildNodes.Where(c => c.Present))
            {
                switch (parseNode.Expression.Identifier)
                {
                    case "scheme":
                        headerValue.Scheme = parseNode.Text;
                        break;
                    case "parameter":
                        headerValue.Parameter.Add(Tavis.Headers.Elements.Parameter.Create(parseNode.ChildNode("parametervalue")));
                        break;
                }
            }

            return headerValue;
        }

    }



}
