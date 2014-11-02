using System.Collections.Generic;

namespace Headers.Parser
{
    public class OptionalExpression : List<IExpression>, IExpression
    {
        public string Identifier { get; private set; }

        public OptionalExpression(string identifier)
        {
            Identifier = identifier;
        }

        public ParseNode Consume(Inputdata input)
        {
            input.Mark();
            var list = new List<ParseNode>();
                
            foreach (var expression in this)
            {
                var result = expression.Consume(input);
                if (result == null)
                {
                    input.MoveToMark();  // If any part of the optional expression fails then it will be considered missing.
                    return new ParseNode(this, "") {NotPresent = true};
                }
                list.Add(result);
            }
           
            var token = new ParseNode(this, "") { ChildNodes = list };
            return token;
        }
    }
}