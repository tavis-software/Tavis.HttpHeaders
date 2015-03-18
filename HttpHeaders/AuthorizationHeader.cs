using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.Parser;

namespace Headers
{
    public class AuthorizationHeaderValue
    {
        private static readonly IExpression _Syntax = new RootExpression("authorization")
            {
                new Token("scheme"),
                new OptionalExpression("parameter") {new Rws(),new Token("parametervalue")}
            };

        public string Scheme { get; set; }
        public string Parameter { get; set; }
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
                        headerValue.Parameter = parseNode.ChildNode("parametervalue").Text;
                        break;
                }
            }

            return headerValue;
        }

    }



}
