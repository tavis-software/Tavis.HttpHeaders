using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
