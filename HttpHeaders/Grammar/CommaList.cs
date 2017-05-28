using Tavis.Parser;

namespace Tavis.Headers
{
    public class CommaList : DelimitedList
    {
        public CommaList(string identifier, IExpression innerExpression)
            : base(identifier, ",", innerExpression)
        {

        }
    }
}