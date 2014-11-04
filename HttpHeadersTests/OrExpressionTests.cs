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
    public class OrExpressionTests
    {


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
