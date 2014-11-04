using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Tavis.Parser;
using Xunit;

namespace HeadersTests
{
    public class TokenTests
    {

        // Parse a token from an input string
        [Fact]
        public void ParseToken()
        {
            var input = new Inputdata( "foo,bar");  // , is a token delimiter

            var token = new Token("boo");
            var parseNode = token.Consume(input);

            Assert.Equal("foo", parseNode.Text);

        }

        [Fact]
        public void ParseTokenDelimitedByEndOfString()
        {
            var input = new Inputdata( "foo");

            var token = new Token("boo");
            var parseToken = token.Consume(input);

            Assert.Equal("foo", parseToken.Text);

        }


        [Fact]
        public void ParseStartingOws()
        {
            var input = new Inputdata("   foo");

            var terminal = new Ows();
            var parseToken = terminal.Consume(input);

            Assert.Equal("   ", parseToken.Text);
            Assert.Equal("foo", input.GetNext(3));
        }

    }
}
