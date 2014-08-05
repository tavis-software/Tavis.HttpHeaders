using System.Collections.Generic;

namespace Headers.Parser
{
    public class Expression : List<IExpression>, IExpression
    {
        public string Identifier { get; private set; }

        public Expression(string identifier = null)
        {
            Identifier = identifier;
        }

        public virtual ParseNode Consume(Inputdata input)
        {
            var list = new List<ParseNode>();
            foreach (var chunk in this)
            {
                var node = chunk.Consume(input);
                if (node != null) list.Add(node);
            }
            var token = new ParseNode(this,"") {ChildNodes = list};
            return token;
        }
    }
}