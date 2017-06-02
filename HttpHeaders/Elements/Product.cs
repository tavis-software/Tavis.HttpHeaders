using System;
using System.Collections.Generic;
using System.Text;

using Tavis.Parser;


namespace Tavis.Headers
{
    public class Product
    {

        public string Name { get; set; }
        public string Version { get; set; }
        public List<string> Comments { get; private set; }
        
        public static readonly IExpression Syntax = new Expression("product")
        {
            new Token("product-token"),
            new OptionalExpression("product-version") {new Literal("/"), new Token("version-token")}
        };


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

        public static Product CreateProduct(ParseNode parseNode)
        {
            var product = new Product();
            product.Name = parseNode.ChildNode("product-token").Text;
            if (parseNode.ChildNode("product-version").Present)
            {
                product.Version = parseNode.ChildNode("product-version").ChildNode("version-token").Text;
            }
            return product;
        }
    }
}