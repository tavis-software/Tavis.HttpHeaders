using Headers.Parser;

namespace Headers
{
    public class CommaList : DelimitedList
    {
        public CommaList(string identifier, Expression innerExpression)
            : base(identifier, ",", innerExpression)
        {

        }
    }
}