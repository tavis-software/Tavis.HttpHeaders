using Tavis.Parser;

namespace Headers
{
    public class CommaList : DelimitedList
    {
        public CommaList(string identifier, IExpression innerExpression)
            : base(identifier, ",", innerExpression)
        {

        }
    }
}