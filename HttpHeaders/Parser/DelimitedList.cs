using System.Collections.Generic;


namespace Tavis.Parser
{
    public class DelimitedList : Expression
    {
        private readonly IExpression _innerExpression;
        private readonly Literal _delimiter;

        public DelimitedList(string identifier, string delimiter, IExpression innerExpression) : base(identifier)
        {
            _innerExpression = innerExpression;

            _delimiter = new Literal(delimiter);
        }


        
        public override ParseNode Consume(Inputdata input)
        {
            var ows = new BasicRule(null,BasicRule.Whitespace);
            var list = new List<ParseNode>();
            do {
                ows.Consume(input);
                list.Add(_innerExpression.Consume(input));
                if (!_delimiter.IsWhitespace()) ows.Consume(input);
            } while (_delimiter.Consume(input) != null) ;

            return new ParseNode(this, "") { ChildNodes = list }; 
        }
    }
}