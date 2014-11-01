using Headers.Parser;

namespace Headers
{
    public class Rws : Terminal
    {
        public Rws()
            : base(null)
        {

        }
        public override ParseNode Consume(Inputdata input)
        {
            var inputChar = ' ';
            input.Mark();
            while (!input.AtEnd && (inputChar == ' ' || inputChar == '\t'))
            {
                inputChar = input.GetNext();
            }
            if (!input.AtEnd) input.MoveBack();
            var capture = input.GetSinceMark();
            var node = new ParseNode(this, capture);
            if (capture.Length == 0)
            {
                node.Error = "Missing required whitespace";
            }
            return node;
        }
    }
}