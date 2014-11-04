using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Tavis.Headers;
using Tavis.Parser;
using Xunit;
using Xunit.Extensions;

namespace Tavis.HeadersTests
{
    public class QValueTest
    {
        [Theory, InlineData("0",0000),
                 InlineData("1", 1000),
                 InlineData("0.1", 100),
                 InlineData("0.123", 123),
                InlineData("1.0", 1000),
                InlineData("1.00", 1000),
                InlineData("1.000", 1000)]
        public void ParseQValue(string text, int iweight)
        {
            decimal weight = iweight/1000M;
            var node = Qvalue.Syntax.Consume(new Inputdata(text));
            var qvalue=  Qvalue.Create(node);
            Assert.Equal(weight,qvalue.Weight);
        }
    }
}
