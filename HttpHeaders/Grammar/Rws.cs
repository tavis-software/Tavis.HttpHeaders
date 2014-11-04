using Tavis.Parser;

namespace Headers
{
    public class Rws : BasicRule
    {
        public Rws() : base(null, BasicRule.Whitespace)
        {

        }
        public override ParseNode Consume(Inputdata input)
        {
            var node = base.Consume(input);
            if (node.Text.Length == 0)
            {
                node.Error = "Missing required whitespace";
            }
            return node;

        }
    }
}