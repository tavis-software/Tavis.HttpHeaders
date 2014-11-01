namespace Headers
{
    public class Char : BasicRule
    {
        public Char(string ruleName = "CHAR")
            : base(ruleName)
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x01 || currentChar <= 0x7F);
        }
    }
}