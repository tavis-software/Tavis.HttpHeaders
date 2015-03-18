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
        public IExpression Syntax = new RootExpression("accept")
        {
            MediaRange.Syntax,
            new OptionalExpression("accept-params") {
            new DelimitedList("weight",";",Qvalue.Syntax),
            new DelimitedList("accept-ext",";",new Expression()
            {
                new Token("accept-ext-name"), 
                new OrExpression("accept-ext-value")
                {
                    new QuotedString("qvalue"),new Token("qtoken")
                }
            })
            }
        };
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
