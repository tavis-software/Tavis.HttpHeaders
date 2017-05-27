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

        [Fact]
        public void Delimited_list_of_quoted_strings()
        {
            var list = new DelimitedList("list", ",", new QuotedString("value"));

            var input = new Inputdata("\"ab,c\",\"def\",\"ghi\"");

            var node = list.Consume(input);
            var items = node.ChildNodes.Select(i => i.Text).ToList();
            Assert.Equal(3, items.Count);
            Assert.Equal("ab,c", items[0]);
            Assert.Equal("ghi", items[2]);
        }
        [Fact]
        public void Delimited_list_of_quoted_key_value_pairs()
        {
            var list = new DelimitedList("list", ",", new Expression("parameter")
                                        {
                                            new Token("name"),
                                            new Ows(),
                                            new Literal("="),
                                            new Ows(),
                                            new QuotedString("value")
                                        });

            var input = new Inputdata("foo=\"ab,c\",bar=\"def\",baz=\"ghi\"");

            var node = list.Consume(input);
            var items = node.ChildNodes.Select(i => new KeyValuePair<string,string>(i["name"].Text,i["value"].Text)).ToList();
            Assert.Equal(3, items.Count);
            Assert.Equal("foo", items[0].Key);
            Assert.Equal("ab,c", items[0].Value);
            Assert.Equal("baz", items[2].Key);
            Assert.Equal("ghi", items[2].Value);
        }
    }
}
