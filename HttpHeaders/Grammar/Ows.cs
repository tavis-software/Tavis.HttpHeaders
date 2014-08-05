using System.ServiceModel.Dispatcher;
using Headers.Parser;

namespace Headers
{
    public class Ows : Terminal
    {
        public Ows() : base(null)
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
            if (capture.Length > 0)
            {
                return new ParseNode(this, capture);
            }
            else
            {
                return null;
            }
            
        }
    }
}