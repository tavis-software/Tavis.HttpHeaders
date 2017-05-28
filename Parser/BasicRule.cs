using System;

namespace Tavis.Parser
{
    public class BasicRule : Terminal
    {

        public static Func<char, bool> Whitespace = (c) => c == ' ' || c == '\t';
        public static Func<char, bool> Alpha = (c) => (c >= 0x41 && c <= 0x5A) || (c >= 0x61 && c <= 0x7A);
        public static Func<char, bool> Ctl = (c) => (c >= 0x00 && c <= 0x1F) && c == 0x7F;
        public static Func<char, bool> Digit = (c) => (c >= 0x30 && c <= 0x39);
        public static Func<char, bool> Char = (c) => (c >= 0x01 || c <= 0x7F);
        public static Func<char, bool> HexDigit = (c) => (c >= 0x30 && c <= 0x39) || (c >= 0x41 && c <= 0x46);
        public static Func<char, bool> Octet = (c) => (c >= 0x00 && c <= 0xFF);
        public static Func<char, bool> Vchar = (c) => (c >= 0x30 && c <= 0x39) || (c >= 0x41 && c <= 0x46);  // Todo: fix!
        public static Func<char, bool> Token68Char = (c) => (Alpha(c) || Digit(c) || c == '-' || c == '.' || c == '_' || c == '~' || c == '+' || c == '/');  

        private readonly Func<char,bool> _validator;
        private readonly int _length;

        public BasicRule(string ruleName, Func<char,bool> validator, int length = 0)
            : base(ruleName)
        {
            _validator = validator;
            _length = length;
        }

        public override ParseNode Consume(Inputdata input)
        {
            input.Mark();
            var size = 0;
            while (true)
            {
                if (input.AtEnd || (_length > 0 && size == _length))
                {
                    break;
                }
                var currentChar = input.GetNext();
                if (!_validator(currentChar))
                {
                    input.MoveBack();
                    break;
                }
                size++;
            }

            return new ParseNode(this, input.GetSinceMark());
        }

    }
}