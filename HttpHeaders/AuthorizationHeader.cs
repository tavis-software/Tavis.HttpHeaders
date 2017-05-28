using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.Headers.Elements;
using Tavis.Parser;

namespace Tavis.Headers
{
    public class AuthorizationHeaderValue
    {
        //credentials = auth-scheme [ 1*SP ( token68 / #auth-param ) ]


        public static Expression Token68Syntax = new Expression("token68") {
                            new BasicRule("token68value",BasicRule.Token68Char),
                            new BasicRule("token68delimiter",c => c == '=',2)
                        };
        public static Expression AuthParameters = new CommaList("parameterpairs", Tavis.Headers.Elements.Parameter.Syntax);

        public string Scheme { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string Token { get; set; }
        public List<string> Errors {get;set;}

        public static AuthorizationHeaderValue Parse(string rawHeaderValue, bool useToken68 = false)
        {
            Expression parameterExpression;
            if (useToken68)
            {
                parameterExpression = Token68Syntax;
            } else
            {
                parameterExpression = AuthParameters;
            }
            IExpression syntax = new RootExpression("authorization")
            {
                new Token("scheme"),

                new OptionalExpression("parameter") {
                    new Literal(" "),
                    parameterExpression
                }
            };
            var node = syntax.Consume(new Inputdata(rawHeaderValue));
           
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
                        var parameterNode = parseNode.ChildNode("token68");
                        if (parameterNode != null)
                        {
                            headerValue.Token = parameterNode["token68value"].Text + "==";
                        } else
                        {
                            var parametersNode = parseNode.ChildNode("parameterpairs");
                            headerValue.Parameters = parametersNode.ChildNodes.Select(n => Tavis.Headers.Elements.Parameter.Create(n)).ToList();
                        }
                        //headerValue.Parameter.Add(Tavis.Headers.Elements.Parameter.Create());
                        break;
                }
            }

            return headerValue;
        }

    }



}
