namespace Headers
{
    public class Octet : BasicRule
    {
        public Octet() : base("OCTET")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x00 && currentChar <= 0xFF);
        }
    }
}