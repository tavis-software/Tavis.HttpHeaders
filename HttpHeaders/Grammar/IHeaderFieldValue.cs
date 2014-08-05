using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Headers
{
    public interface  IHeaderFieldValue
    {
        void Parse(string rawValue);
    }
}
