using Tavis.Parser;

namespace Tavis.Headers
{
    public class SemiColonList : DelimitedList
    {
        public SemiColonList(string identifier, IExpression innerExpression)
            : base(identifier, ";", innerExpression)
        {
            
        }
    }

    //public class DelimitedList : Expression
    //{
    //    private readonly Expression _innerExpression;
    //    private string _literal;


    //    public DelimitedList(string delimiter, Expression innerExpression)
    //    {
    //        _innerExpression = innerExpression;

    //        _literal = delimiter;
    //    }

    //    public override ParseNode Consume(Inputdata input)
    //    {
    //        var ows = new Ows();
    //        ows.Consume(input);
    //        var delimiter = new Literal(_literal);
    //        var list = new List<ParseNode>();
    //        while (delimiter.Consume(input) != null)
    //        {
    //            ows.Consume(input);

    //            list.Add(_innerExpression.Consume(input));

    //            ows.Consume(input);
    //        }

    //        return new ParseNode(null, ""){ChildNodes = list};;
    //    }
    //}
}