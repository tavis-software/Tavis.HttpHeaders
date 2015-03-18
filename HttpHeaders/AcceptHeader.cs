using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Tavis.Headers.Elements;
using Tavis.Parser;

namespace Tavis.Headers
{
    //Accept = #( media-range [ accept-params ] )
 
    
    //  media-range    = ( "*/*"
    //                  / ( type "/" "*" )
    //                  / ( type "/" subtype )
    //                  ) *( OWS ";" OWS parameter )

    // accept-params  = weight *( accept-ext )
    // accept-ext = OWS ";" OWS token [ "=" ( token / quoted-string ) ]
    // weight = OWS ";" OWS "q=" qvalue
    // qvalue = ( "0" [ "." *3DIGIT ] ) / ( "1" [ "." *3"0" ] )

    public class AcceptHeader
    {
        public List<string> Errors { get; set; }
        public List<WeightedMediaRange> MediaRanges { get; set; }
 
        private static IExpression _Syntax = new RootExpression("accept")
        {
            new CommaList("media-range-list", new Expression("media-range-item") { 
                MediaRange.Syntax,
                new OptionalExpression("accept-params") {
                    new DelimitedList("weight",";",Qvalue.Syntax),
                    new DelimitedList("accept-ext",";",new Expression()
                    {
                        new Token("accept-ext-name"), 
                        new OptionalExpression("accept-ext-right") {
                            new Literal("="),
                            new OrExpression("accept-ext-value")
                            {
                                new QuotedString("qvalue"),new Token("qtoken")
                            }
                        }
                    })
                }
            })
        };

        public class WeightedMediaRange
        {
            
        }

        public static AcceptHeader Parse(string rawHeaderValue)
        {
            var node = _Syntax.Consume(new Inputdata(rawHeaderValue));

            var headerValue = new AcceptHeader
            {
                Errors = node.GetErrors()
            };

            foreach (var parseNode in node.ChildNodes.Where(c => c.Present))
            {
                switch (parseNode.Expression.Identifier)
                {
                    case "media-range-item":
                        //headerValue.Scheme = parseNode.Text;
                        break;
                }
            }

            return headerValue;
        }
    }
    
    public class MediaRange
    {
        public static IExpression Syntax = new Expression("media-range")
        {
            new OrExpression("media-range-value")
            {
                new Literal("*/*"),
                new Expression() { new Token("type"), new Literal("/*")},
                MediaType.Syntax
            },
            new DelimitedList("parameters",";",Parameter.Syntax)
        };
    }
}
