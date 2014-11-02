using System;
using System.ServiceModel.Dispatcher;
using Headers.Parser;


namespace Headers

{
    public class Ows : BasicRule
    {
        public Ows() : base(null,BasicRule.Whitespace)
        {
            
        }
    }

}