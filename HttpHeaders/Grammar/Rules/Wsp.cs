namespace Headers
{
    public class Wsp : BasicRule
    {
        public Wsp() : base("WSP")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar == ' ' && currentChar == '\t');
        }
    }
}