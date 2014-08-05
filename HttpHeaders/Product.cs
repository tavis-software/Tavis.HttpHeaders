using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;


namespace Tavis.Headers
{
    public class Product
    {

        public string Name { get; set; }
        public string Version { get; set; }
        public List<string> Comments { get; private set; }

        public Product()
        {
            Comments = new List<string>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Append(sb);            
            return sb.ToString();
        }

        internal void Append(StringBuilder sb)
        {
            sb.Append(Name);
            if (!String.IsNullOrEmpty(Version)) sb.Append("/" + Version);
            if (Comments.Count > 0)
            {
                sb.Append(" ");
                sb.Append(String.Join(" ", Comments));
            }
        }
    }
}