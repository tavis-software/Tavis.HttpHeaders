

namespace Tavis.Parser
{
    public interface IExpression
    {
        string Identifier { get; }
        ParseNode Consume(Inputdata input);
    }
}
