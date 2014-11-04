using System;
using System.Collections.Generic;
using Tavis.Parser;

namespace Headers
{

    public class Token : BasicRule
    {
       public static HashSet<char> HttpTokenDelimiters = new HashSet<char>() {',', '/','(',')',',',';','<','=','>','?','@','[','\\',']','{','}','"',' ','\t'};

       public Token(string identifier)
           : base(identifier, (c) => !HttpTokenDelimiters.Contains(c))
        {
            
        }
    }


    
}