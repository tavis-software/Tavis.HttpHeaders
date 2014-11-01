namespace Headers
{
    public class Digit : BasicRule
    {
        public Digit(int length = 0) : base("DIGIT",length)
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x30 && currentChar <= 0x39);
        }
    }
}