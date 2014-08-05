using System.Collections.Generic;

namespace Headers.Parser
{
    public abstract class Terminal : IExpression
    {
        public string Identifier { get; private set; }

        public Terminal(string identifier)
        {
            Identifier = identifier;
        }

        public abstract ParseNode Consume(Inputdata input);

        ParseNode IExpression.Consume(Inputdata input)
        {
            return Consume(input);
        }
    }
}