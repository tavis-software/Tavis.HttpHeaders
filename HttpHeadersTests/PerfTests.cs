using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Tavis.Headers;
using Xunit;

namespace HeadersTests
{
    public class PerfTests
    {
        public const int Iterations = 10000;

        [Fact]
        public void AuthHeaderPerf()
        {
            for (int i = 0; i < Iterations; i++)
            {
                var authHeader = AuthorizationHeaderValue.Parse("foo asdasdasdasdasdasd");

            }    
        }

        [Fact]
        public void UserAgentHeaderPerf()
        {
            for (int i = 0; i < Iterations; i++)
            {
                var userAgentHeader = UserAgentHeaderValue.Parse("Mozilla/4.0 (compatible; Linux 2.6.22) NetFront/3.4 Kindle/2.5 (screen 824x1200;rotate)");

            }
        }
    }
}
