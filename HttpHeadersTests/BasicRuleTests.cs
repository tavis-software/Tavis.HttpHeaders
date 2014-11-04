using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Tavis.Parser;
using Xunit;

namespace Tavis.HeadersTests
{
    public class BasicRuleTests
    {

        [Fact]
        public void Should_parse_alpha_value()
        {

            var alpha = new BasicRule("ALPHA", BasicRule.Alpha);

            var input = new Inputdata("hereIsSomeAlpha!and other stuff");

            var node = alpha.Consume(input);
            Assert.Equal("hereIsSomeAlpha", node.Text);
        }

        [Fact]
        public void Should_parse_alpha_value_()
        {

            var alpha = new BasicRule("ALPHA",BasicRule.Alpha);

            var input = new Inputdata("hereIsSomeAlpha!and other stuff");

            var node = alpha.Consume(input);
            Assert.Equal("hereIsSomeAlpha", node.Text);
        }

        [Fact]
        public void Should_return_empty_if_no_alpha_value()
        {

            var alpha = new BasicRule("ALPHA", BasicRule.Alpha); ;

            var input = new Inputdata("@#hereIsSomeAlpha!and other stuff");

            var node = alpha.Consume(input);
            Assert.Equal("", node.Text);
        }


        [Fact]
        public void Should_parse_Digit_value_up_to_length()
        {

            var alpha = new BasicRule("DIGIT",BasicRule.Digit,3);

            var input = new Inputdata("8392323");

            var node = alpha.Consume(input);
            Assert.Equal("839", node.Text);
        }

    }
}
