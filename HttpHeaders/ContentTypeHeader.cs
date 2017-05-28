using System.Collections.Generic;

using Tavis.Headers.Elements;
using Tavis.Parser;

namespace Tavis.Headers
{

    //   media-type = type "/" subtype *( OWS ";" OWS parameter )
    // type = token
    // subtype = token
    // parameter = token "=" ( token / quoted-string )
    public class ContentTypeHeaderValue
    {
    
        public MediaType MediaType { get; set; }
        public Dictionary<string,string> Parameters { get; set; }
 
        private static readonly IExpression _Syntax = new Expression("contenttype")
            {
                new Expression("mediatype")
                {
                    new Token("type"),
                    new Literal("/"),
                    new Token("subtype"),

                },
                new Literal(";"),
                new SemiColonList("parameters", new Expression("parameter")
                {
                    new Token("name"),
                    new Ows(),
                    new Literal("="),
                    new Ows(),
                    new Token("value")
                })
            };

        public ContentTypeHeaderValue()
        {
            Parameters = new Dictionary<string, string>();
        }
        public static ContentTypeHeaderValue Parse(string rawHeaderValue)
        {
            var headerValue = new ContentTypeHeaderValue();
            var node = _Syntax.Consume(new Inputdata(rawHeaderValue));

            var mediaTypeNode = node.ChildNode("mediatype");
            headerValue.MediaType = MediaType.Create(mediaTypeNode);
            var parameters = node.ChildNode("parameters");
            
            foreach (var parameterNode in parameters.ChildNodes)
            {
                headerValue.Parameters[parameterNode.ChildNode("name").Text] = parameterNode.ChildNode("value").Text;
            }
            return headerValue;
        }
    }
}
