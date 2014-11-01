namespace Headers
{
    public class Alpha : BasicRule
    {
        public Alpha() : base("ALPHA")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x41 && currentChar <= 0x5A) || (currentChar >= 0x61 && currentChar <= 0x7A);
        }
    }
}