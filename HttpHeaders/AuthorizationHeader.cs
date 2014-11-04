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
        private static readonly IExpression _Syntax = new Expression("authorization")
            {
                new Token("scheme"),
                new Rws(),
                new Token("parameter")
            };

        public string Scheme { get; set; }
        public string Parameter { get; set; }

        public static AuthorizationHeaderValue Parse(string rawHeaderValue)
        {
            var node = _Syntax.Consume(new Inputdata(rawHeaderValue));
            var headerValue = new AuthorizationHeaderValue();
            foreach (var parseNode in node.ChildNodes)
            {
                switch (parseNode.Expression.Identifier)
                {
                    case "scheme":
                        headerValue.Scheme = parseNode.Text;
                        break;
                    case "parameter":
                        headerValue.Parameter = parseNode.Text;
                        break;
                }
            }

            return headerValue;
        }

    }



}
