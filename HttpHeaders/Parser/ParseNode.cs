using System.Collections.Generic;
using System.Linq;

namespace Headers.Parser
{
    public class ParseNode 
    {
        public IExpression Expression { get; private set; }
        public string Text { get; private set; }
        public List<ParseNode> ChildNodes { get; set; }
        public bool NotPresent { get; set; }
        public string Error { get; set; }

        public ParseNode(IExpression expression, string text)
        {
            Expression = expression;
            Text = text;
            
        }


        public ParseNode ChildNodeContains(string identifier, string value)
        {
            return ChildNodes.FirstOrDefault(
                        parameter => parameter.ChildNodes.Where(n => n.Expression.Identifier == identifier).Any(n => n.Text == value));
            
        }
        
        public ParseNode ChildNode(string identifier)
        {
            return ChildNodes.FirstOrDefault(n => n.Expression.Identifier == identifier);
        }
    }
}
