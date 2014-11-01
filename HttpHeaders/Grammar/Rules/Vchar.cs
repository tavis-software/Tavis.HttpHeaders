namespace Headers
{
    public class Vchar : BasicRule
    {
        public Vchar(string ruleName = "VCHAR") : base(ruleName)
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x30 && currentChar <= 0x39) || (currentChar >= 0x41 && currentChar <= 0x46);
        }
    }
}