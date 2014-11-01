namespace Headers
{
    public class Ctl : BasicRule
    {
        public Ctl()
            : base("CTL")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x00 && currentChar <= 0x1F) && currentChar == 0x7F;
        }
    }
}