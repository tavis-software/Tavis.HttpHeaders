using System;
using Headers.Parser;

namespace Headers
{
    public class Literal : Terminal
    {
        private readonly string _literal;

        public Literal(string literal) : base(Guid.NewGuid().ToString())
        {
            _literal = literal;
        }

        public bool IsWhitespace()
        {
            return String.IsNullOrWhiteSpace(_literal);
        }
        public override ParseNode Consume(Inputdata input)
        {
            input.Mark();
            if (!IsValid(input.GetNext(_literal.Length)))
            {
                input.MoveToMark();
                return null;
            }
            return new ParseNode(this,input.GetSinceMark());
        }

        private bool IsValid(string input)
        {
            return _literal == input;
        }
    }
}