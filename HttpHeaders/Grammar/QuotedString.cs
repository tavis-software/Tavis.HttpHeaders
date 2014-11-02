using System.Runtime.CompilerServices;

namespace Headers
{

    public class QuotedString : DelimitedToken
    {
        public QuotedString(string identifier) : base(identifier,'"','"')
        {
            
        }
    }
}