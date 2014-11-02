using Headers.Parser;

namespace Headers
{
    public class DelimitedToken : Terminal
    {
        private readonly char _endDelimiter;
        private readonly char _startDelimiter;

        public DelimitedToken(string identifier, char startDelimiter, char endDelimiter) : base(identifier)
        {
            _startDelimiter = startDelimiter;
            _endDelimiter = endDelimiter;
        }

        public override ParseNode Consume(Inputdata input)
        {

            var firstChar = input.GetNext();
            
            if (firstChar != _startDelimiter)
            {
                return null;
            }
            input.Mark();

            var delimfound = false;
            while (!delimfound)
            {
                if (input.AtEnd)
                {
                    delimfound = true;
                }
                else
                {
                    var currentChar = input.GetNext();
                    if (currentChar == _endDelimiter)
                    {
                        delimfound = true;
                        input.MoveBack();
                    }
                }
            }
            var text = input.GetSinceMark();
            input.GetNext(); //Don't allow end delimiter to be captured
            return new ParseNode(this, text);
        }
    }
}