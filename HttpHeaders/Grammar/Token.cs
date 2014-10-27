using System;
using System.Xml;
using Headers.Parser;

namespace Headers
{
    public class Token : Terminal
    {
     
        public Token(string identifier) : base(identifier)
        {
          
        }

        public override ParseNode Consume(Inputdata input)
        {
            input.Mark();
            var delimfound = false;
            while (!delimfound)
            {
                if (input.AtEnd)
                {
                    delimfound = true;
                }
                else
                {
                    var currentChar = input.GetNext(); 
                    switch (currentChar)
                    {
                        case ',': 
                        case '/':
                        case '(':
                        case ')':
                        case ':':
                        case ';':
                        case '<':
                        case '=':
                        case '>':
                        case '?':
                        case '@':
                        case '[':
                        case '\\':
                        case ']':
                        case '{':
                        case '}':
                        case '"':
                        case ' ':
                        case '\t':
                            delimfound = true;
                            input.MoveBack();
                            break;
                    }
                }
            }
            var text = input.GetSinceMark();
            return new ParseNode(this,text);
        }
    }

    
    public abstract class BasicRule : Terminal
    {
        private readonly int _length;

        public BasicRule(string ruleName, int length = 0)
            : base(ruleName)
        {
            _length = length;
        }

        public override ParseNode Consume(Inputdata input)
        {
            input.Mark();
            var delimfound = false;
            var size = 0;
            while (!delimfound )
            {
                if (input.AtEnd || (_length > 0 && size == _length))
                {
                    delimfound = true;
                }
                else 
                {
                    var currentChar = input.GetNext();
                    if (!IsValidChar(currentChar))
                    {
                        delimfound = true;
                        input.MoveBack();
                    }
                    size++;
                }
            }
            var text = input.GetSinceMark();
            return new ParseNode(this, text);
        }

        protected abstract bool IsValidChar(char currentChar);
    }

    public class Char : BasicRule
    {
        public Char() : base("CHAR")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x01 || currentChar <= 0x7F);
        }
    }

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


    public class HexDigit : BasicRule
    {
        public HexDigit() : base("HEXDIG")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x30 && currentChar <= 0x39) || (currentChar >= 0x41 && currentChar <= 0x46);
        }
    }

    public class Vchar : BasicRule
    {
        public Vchar() : base("VCHAR")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar >= 0x30 && currentChar <= 0x39) || (currentChar >= 0x41 && currentChar <= 0x46);
        }
    }
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

    public class WSP : BasicRule
    {
        public WSP() : base("WSP")
        {
        }

        protected override bool IsValidChar(char currentChar)
        {
            return (currentChar == ' ' && currentChar == '\t');
        }
    }

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

    public class Cr : Literal
    {
        public Cr() :base("\r")
        {
        }
    }

    public class CrLf : Literal
    {
        public CrLf() : base("\r\n")
        {
        }
    }

    public class Lf : Literal
    {
        public Lf() : base("\n")
        {
        }
    }

    public class Sp : Literal
    {
        public Sp()
            : base(" ")
        {
        }
    }

    public class DQuote : Literal
    {
        public DQuote()
            : base("\"")
        {
        }
    }

}