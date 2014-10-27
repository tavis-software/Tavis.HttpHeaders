using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavis.Headers
{
    // Host = uri-host [ ":" port ]
    // uri-host = IP-literal / IPv4address / reg-name
    // IP-literal = "[" ( IPv6address / IPvFuture  ) "]"/
    // IPvFuture  = "v" 1*HEXDIG "." 1*( unreserved / sub-delims / ":" )
    // IPv4address = dec-octet "." dec-octet "." dec-octet "." dec-octet
    // dec-octet   = DIGIT                 ; 0-9
    //              / %x31-39 DIGIT         ; 10-99
    //              / "1" 2DIGIT            ; 100-199
    //              / "2" %x30-34 DIGIT     ; 200-249
    //              / "25" %x30-35          ; 250-255
    // IPv6address =                            6( h16 ":" ) ls32
    //              /                       "::" 5( h16 ":" ) ls32
    //              / [               h16 ] "::" 4( h16 ":" ) ls32
    //              / [ *1( h16 ":" ) h16 ] "::" 3( h16 ":" ) ls32
    //              / [ *2( h16 ":" ) h16 ] "::" 2( h16 ":" ) ls32
    //              / [ *3( h16 ":" ) h16 ] "::"    h16 ":"   ls32
    //              / [ *4( h16 ":" ) h16 ] "::"              ls32
    //              / [ *5( h16 ":" ) h16 ] "::"              h16
    //              / [ *6( h16 ":" ) h16 ] "::"

    //  ls32        = ( h16 ":" h16 ) / IPv4address
    //              ; least-significant 32 bits of address

    //  h16         = 1*4HEXDIG
    //              ; 16 bits of address represented in hexadecimal
    // unreserved  = ALPHA / DIGIT / "-" / "." / "_" / "~"

    // sub-delims  = "!" / "$" / "&" / "'" / "(" / ")"
    //              / "*" / "+" / "," / ";" / "="
    // reg-name    = *( unreserved / pct-encoded / sub-delims )
    // pct-encoded = "%" HEXDIG HEXDIG



    public class HostHeader
    {
    }
}
