using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavis.Headers;
using Xunit;

namespace Tavis.HeadersTests
{
    public class AcceptHeaderTests
    {
        [Fact(Skip = "To complete")]
        public void Single_wildcard()
        {
            var acceptHeader = AcceptHeader.Parse("*/*");
            Assert.Equal(1, acceptHeader.MediaRanges.Count);
        }
    }
}
