using System.Collections.Generic;

namespace Tavis.Parser
{
    public class OrExpression : List<IExpression>, IExpression
    {
        public string Identifier { get; private set; }


        public OrExpression(string identifier)
        {
            Identifier = identifier;
        }

        public ParseNode Consume(Inputdata input)
        {
            input.Mark();

            foreach (var expression in this)
            {
                var result = expression.Consume(input);
                if (result != null && result.Present && result.Error == null)
                {
                    return result;
                }
                input.MoveToMark();
            }
            return null;
        }
    }
}