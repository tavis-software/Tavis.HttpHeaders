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
            if (!(inputChar == ' ' || inputChar == '\t')) input.MoveBack();
            var capture = input.GetSinceMark();
            return new ParseNode(this, capture);
           
        }
    }
}