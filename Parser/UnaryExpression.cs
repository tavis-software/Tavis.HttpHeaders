namespace Tavis.Parser
{
    public class UnaryExpression : IExpression
    {
        private readonly Terminal _terminal;

        public UnaryExpression(Terminal terminal)
        {
            Identifier = terminal.Identifier;
            _terminal = terminal;
        }

        public string Identifier { get; private set; }

        public ParseNode Consume(Inputdata input)
        {
            return _terminal.Consume(input);
        }
    }
}