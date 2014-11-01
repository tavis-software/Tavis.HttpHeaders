using Headers.Parser;

namespace Headers
{
    public class QuotedString : Terminal
    {
        public QuotedString(string identifier)
            : base(identifier)
        {
        }

        public override ParseNode Consume(Inputdata input)
        {
            

            var firstChar = input.GetNext();
            if (firstChar != '"')
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
                    switch (currentChar)
                    {
                        case '"':
                            delimfound = true;
                            input.MoveBack();
                            break;
                    }
                }
            }
            var text = input.GetSinceMark();
            input.GetNext(); //Throw away closing quote
            return new ParseNode(this, text);
        }
    }
}