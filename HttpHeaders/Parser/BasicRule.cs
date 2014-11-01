using Headers.Parser;

namespace Headers
{
    public abstract class BasicRule : Terminal
    {
        private readonly int _length;

        public BasicRule(string ruleName, int length = 0)
            : base(ruleName)
        {
            _length = length;
        }

        public override ParseNode Consume(Inputdata input)
        {
            input.Mark();
            var delimfound = false;
            var size = 0;
            while (!delimfound )
            {
                if (input.AtEnd || (_length > 0 && size == _length))
                {
                    delimfound = true;
                }
                else 
                {
                    var currentChar = input.GetNext();
                    if (!IsValidChar(currentChar))
                    {
                        delimfound = true;
                        input.MoveBack();
                    }
                    size++;
                }
            }
            var text = input.GetSinceMark();
            return new ParseNode(this, text);
        }

        protected abstract bool IsValidChar(char currentChar);
    }
}