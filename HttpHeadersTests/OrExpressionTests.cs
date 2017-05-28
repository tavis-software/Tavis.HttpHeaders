using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.Headers;
using Tavis.Headers.Elements;
using Tavis.Parser;
using Xunit;

namespace Tavis.HeadersTests
{
    public class OrExpressionTests
    {
        public static IExpression CommandSyntax = new Expression("cmd")
            {
                new Ows(),
                new Literal("runscope:"),
                new Ows(),
                new Token("verb"),
                new Rws(),
                new OrExpression("bucket") {
                    new QuotedString("qbucket"),
                    new Token("tbucket")
                },
                new Ows(),
                new OptionalExpression("testphrase")
                {
                    new Literal("/"),
                    new Ows(),
                    new QuotedString("test")
                },
                new OptionalExpression("paramlist")
                {
                    new Rws(),
                    new Literal("with"),
                    new CommaList("parameters",Parameter.Syntax)    
                }
                
            };

        [Fact]
        public void DelimitedListWithOrCondition()
        {
            var fooexpr = new OrExpression("xx")
            {
                new Comment("type"),
                new Token("test")
            };
            var rule = new DelimitedList(null, " ", fooexpr);

            var tokens = rule.Consume(new Inputdata(" blah (bob) glick glob (foo)")).ChildNodes;

            Assert.Equal(5, tokens.Count());
        }

        [Fact]
        public void QuotedOrToken()
        {
            var rule = new OrExpression("xx")
            {
                new QuotedString("type"),
                new Token("test")
            };
            

            var token = rule.Consume(new Inputdata("blah"));

            Assert.Equal("blah", token.Text);

            var quoted = rule.Consume(new Inputdata("\"blah\""));

            Assert.Equal("blah", quoted.Text);
        
        }

        [Fact]
        public void Commandtest()
        {
            IExpression commandSyntax = new Expression("cmd")
            {
                
                new OrExpression("bucket") {
                    new QuotedString("qbucket"),
                    new Token("tbucket")
                },
                new Ows(),
                new OptionalExpression("testphrase")
                {
                    new Literal("/"),
                    new Ows(),
                    new OrExpression("test") {
                        new QuotedString("qtest"),
                        new Token("ttest")
                    }
                }
                
            };

            var token = commandSyntax.Consume(new Inputdata("foo/bar"));

            Assert.Equal("foo",token.ChildNode("tbucket").Text);
            Assert.Equal("bar",token.ChildNode("testphrase").ChildNode("ttest").Text);
        }

        [Fact]
        public void Commandtest2()
        {
            var rule = new Expression("cmd")
            {
                new Token("verb"),
                new Rws(),
                new OrExpression("bucket") {
                    new QuotedString("qbucket"),
                    new Token("tbucket")
                },
                new Ows(),
            };



            var token = rule.Consume(new Inputdata("run \"foo\""));

           // Assert.NotNull(token.ChildNodes["tbucket"]);

        }

        [Fact]
        public void DelimitedListWithOrConditionThatContainsExpressions()
        {
            var fooexpr = new OrExpression("xx")
            {
                new Expression() {new Literal("!"), new Comment("type") },
                new Expression() {new Literal("@"), new Token("test") }
            };
            var rule = new DelimitedList(null, " ", fooexpr);

            var tokens = rule.Consume(new Inputdata(" @blah !(bob) @glick @glob !(foo)")).ChildNodes;

            Assert.Equal(5, tokens.Count());
        }
    }
}
