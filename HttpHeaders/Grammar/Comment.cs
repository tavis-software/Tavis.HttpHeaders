using Tavis.Parser;

namespace Tavis.Headers
{
    public class Comment : DelimitedToken
    {
        public Comment(string identifier) : base(identifier, '(',')')
        {
            
        }    
    }
    //public class Comment : Terminal
    //{
    //    public Comment(string identifier) : base(identifier) {
    //    }

    //    public override ParseNode Consume(Inputdata input)
    //    {
    //        input.Mark();

    //        var firstChar = input.GetNext();
    //        if (firstChar != '(')
    //        {
    //            input.MoveBack();
    //            return null;
    //        }

    //        var delimfound = false;
    //        while (!delimfound)
    //        {
    //            if (input.AtEnd)
    //            {
    //                delimfound = true;
    //            }
    //            else
    //            {
    //                var currentChar = input.GetNext();
    //                switch (currentChar)
    //                {
    //                    case ')':
    //                        delimfound = true;
    //                        //input.MoveBack();
    //                        break;
    //                }
    //            }
    //        }
    //        var text = input.GetSinceMark();
    //        return new ParseNode(this, text);
    //    }
    //}
}