using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Tavis.Headers;
using Tavis.Parser;
using Xunit;

namespace HeadersTests
{
    public class PerfTests
    {
        public const int Iterations = 10000;

        [Fact]
        public void AuthHeaderPerf()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            for (int i = 0; i < Iterations; i++)
            {
                var authHeader = AuthorizationHeaderValue.Parse("foo asdasdasdasdasdasd==",useToken68: true);

            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed for auth header parsing " + stopwatch.ElapsedMilliseconds);

        
        }

        [Fact]
        public void UserAgentHeaderPerf()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < Iterations; i++)
            {
                var userAgentHeader = UserAgentHeaderValue.Parse("Mozilla/4.0 (compatible; Linux 2.6.22) NetFront/3.4 Kindle/2.5 (screen 824x1200;rotate)");

            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed for useragent parsing " + stopwatch.ElapsedMilliseconds);

        }

        [Fact]
        public void TokenPerfHashSet()
        {
            var expression = new Expression("foo") { new Token("a"), new Literal(","), new Token("b"), new Literal(","), new Token("c") };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < Iterations; i++)
            {

                var result = expression.Consume(new Inputdata("aweasds23,asdsdwe34343,dfefefsdsd"));

            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed for hashset based token " + stopwatch.ElapsedMilliseconds);


        }

    }
}
