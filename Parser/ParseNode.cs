﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tavis.Parser
{
    [DebuggerDisplay("Identifier = {Expression.Identifier}")]
    public class ParseNode 
    {
        public IExpression Expression { get; private set; }
        public string Text { get; private set; }
        public List<ParseNode> ChildNodes { get; set; }
        public bool Present { get; set; }
        public string Error { get; set; }

        public ParseNode(IExpression expression, string text)
        {
            Expression = expression;
            Text = text;
            Present = true;

        }


        public ParseNode ChildNodeContains(string identifier, string value)
        {
            return ChildNodes.FirstOrDefault(
                        parameter => parameter.ChildNodes.Where(n => n.Expression.Identifier == identifier).Any(n => n.Text == value));
            
        }

        public ParseNode this[string identifier] 
        {
            get { return ChildNode(identifier); }
        }
        public ParseNode ChildNode(string identifier)
        {
            return ChildNodes.FirstOrDefault(n => n.Expression.Identifier == identifier);
        }
    }
}
