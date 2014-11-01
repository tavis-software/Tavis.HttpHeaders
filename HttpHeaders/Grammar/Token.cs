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
}