using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headers.Parser
{
    public interface IExpression
    {
        string Identifier { get; }
        ParseNode Consume(Inputdata input);
    }
}
