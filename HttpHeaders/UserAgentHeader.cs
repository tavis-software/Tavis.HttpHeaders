using System.Collections.Generic;
using System.Linq;
using System.Text;
using Headers;
using Headers.Parser;

namespace Tavis.Headers
{
    public class UserAgentHeaderValue
    {

        public List<Product> Products { get; private set; }

        public UserAgentHeaderValue()
        {
            Products = new List<Product>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var product in Products)
            {
                product.Append(sb);
                if (product != Products.Last()) sb.Append(" ");
            }
            return sb.ToString();
        }

        private static IExpression _Syntax = new Expression("useragent")
        {
            new Token("product"),
            new OptionalExpression("versionex") {new Literal("/"), new Token("version")}, //Optional version
            new Rws(),
            new DelimitedList("productorcomments", " ", new Expression("token")
            {
                new Ows(),
                new OrExpression("productorcomment")
                {
                    new Comment("comment"),
                    new Expression("productex")
                    {
                        new Token("product"),
                        new OptionalExpression("versionex") {new Literal("/"), new Token("version")}
                    }
                }
            })
        };

        public static UserAgentHeaderValue Parse(string rawHeaderValue)
        {
            var node = _Syntax.Consume(new Inputdata(rawHeaderValue));
            var headerValue = new UserAgentHeaderValue();

            if (node.ChildNode("product") != null)
            {
                headerValue.Products.Add(CreateProduct(node));
            }
            if (node.ChildNode("productorcomments") != null)
            {
                foreach (var childnode in node.ChildNode("productorcomments").ChildNodes)
                {
                    if (childnode.ChildNode("comment") != null)
                    {
                        var lastProduct = headerValue.Products.Last();
                        lastProduct.Comments.Add(childnode.ChildNode("comment").Text);
                    }
                    else
                    {
                        headerValue.Products.Add(CreateProduct(childnode.ChildNode("productex")));
                    }
                }
            }

            return headerValue;
        }

        private static Product CreateProduct(ParseNode parseNode)
        {
            var product = new Product();
            product.Name = parseNode.ChildNode("product").Text;
            if (parseNode.ChildNode("versionex").NotPresent == false)
            {
                product.Version = parseNode.ChildNode("versionex").ChildNode("version").Text;
            }
            return product;
        }
    }
}
