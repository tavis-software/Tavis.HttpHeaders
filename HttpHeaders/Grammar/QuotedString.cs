using System.Runtime.CompilerServices;
using Tavis.Parser;

namespace Tavis.Headers
{

    public class QuotedString : DelimitedToken
    {
        public QuotedString(string identifier) : base(identifier,'"','"')
        {
            
        }
    }
}