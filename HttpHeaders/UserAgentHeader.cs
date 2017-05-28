using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tavis.Parser;

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
            Product.Syntax,
            new Rws(),
            new DelimitedList("productorcomments", " ", new Expression("token")
            {
                new Ows(),
                new OrExpression("productorcomment")
                {
                    new Comment("comment"),
                    Product.Syntax
                }
            })
        };

        public static UserAgentHeaderValue Parse(string rawHeaderValue)
        {
            var node = _Syntax.Consume(new Inputdata(rawHeaderValue));
            var headerValue = new UserAgentHeaderValue();

            if (node["product"] != null)
            {
                headerValue.Products.Add(Product.CreateProduct(node["product"]));
            }
            if (node["productorcomments"] != null)
            {
                foreach (var childnode in node["productorcomments"].ChildNodes)
                {
                    if (childnode["comment"] != null)
                    {
                        var lastProduct = headerValue.Products.Last();
                        lastProduct.Comments.Add(childnode["comment"].Text);
                    }
                    else
                    {
                        headerValue.Products.Add(Product.CreateProduct(childnode["product"]));
                    }
                }
            }

            return headerValue;
        }

   
    }
}
