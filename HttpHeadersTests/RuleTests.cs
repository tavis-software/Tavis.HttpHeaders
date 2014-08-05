using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Headers.Parser;
using Tavis.Headers;
using Xunit;
using Xunit.Extensions;

namespace HeadersTests
{
    public class RuleTests
    {
        [Fact]
        public void SimpleRule()
        {
            var rule = new Expression()
            {
                new Token("type"),
                new Literal("/"),
                new Token("subtype")
            };

            var tokens = rule.Consume(new Inputdata("application/xml"));

            var type = tokens.ChildNodes.First(t => t.Expression.Identifier == "type");
            var subtype = tokens.ChildNodes.First(t => t.Expression.Identifier == "subtype");

            Assert.Equal("application", type.Text);
            Assert.Equal("xml", subtype.Text);
        }

        [Fact]
        public void SemiColonListOfTokens()
        {
            var rule = new SemiColonList(null, new Expression() {new Token("foo")});

            var tokens = rule.Consume(new Inputdata(" bar  ; baz  ; blur  ; blog")).ChildNodes;


            Assert.Equal(4, tokens.Count());
            Assert.Equal("bar", tokens.First().ChildNodes.First().Text);
            Assert.Equal("blog", tokens.Last().ChildNodes.First().Text);
        }

        [Fact]
        public void Accept()
        {
            var mediatype = new Expression()
            {
                new Token("type"),
                new Literal("/"),
                new Token("subtype")
            };
            var rule = new DelimitedList(null, ",", mediatype);

            var tokens = rule.Consume(new Inputdata(" application/xml  , text/html,  application/json")).ChildNodes;

            Assert.Equal(3, tokens.Count());
        }

        
        [Theory, InlineData("charset=utf-8"),
        InlineData(" charset=utf-8"),
        InlineData("charset =utf-8"),
        InlineData("charset= utf-8"),
        InlineData("charset=utf-8 "),
        InlineData(" charset =utf-8"),
        InlineData(" charset = utf-8"),
        InlineData(" charset = utf-8 "),
        InlineData("charset = utf-8 "),
        InlineData("charset= utf-8 "),
        InlineData("charset=utf-8 ")
        ]
        public void ParseParameterValue(string input)
        {
            var parameter = new Expression
            {
                new Token("name"),
                new Ows(),
                new Literal("="),
                new Ows(),
                new Token("value")
            };

            var parsenode = parameter.Consume(new Inputdata("charset=utf-8"));

            var name =
                parsenode.ChildNodes.Where(p => p.Expression != null && p.Expression.Identifier == "name").FirstOrDefault();
            var value =
                parsenode.ChildNodes.Where(p => p.Expression != null && p.Expression.Identifier == "value").FirstOrDefault();

            Assert.Equal("charset",name.Text);
            Assert.Equal("utf-8", value.Text);
        }

        [Fact]
        public void ContentType()
        {
            
            var contentType = new Expression("contenttype")
            {
                new Expression("mediatype")
                {
                    new Token("type"),
                    new Literal("/"),
                    new Token("subtype"),

                },
                new Literal(";"),
                new SemiColonList("parameters", new Expression("parameter")
                {
                    new Token("name"),
                    new Ows(),
                    new Literal("="),
                    new Ows(),
                    new Token("value")
                })
            };

            var contentTypeNode = contentType.Consume(new Inputdata("application/xml;charset=utf-8;schema=false"));

            var mediatypeNode = contentTypeNode.ChildNode("mediatype");
            var parametersNode = contentTypeNode.ChildNode("parameters");
            var charsetParameter = parametersNode.ChildNodeContains("name", "charset");
            var schemaParameter = parametersNode.ChildNodeContains("name", "schema");


            Assert.NotNull(mediatypeNode);
            Assert.NotNull(parametersNode);
            Assert.Equal("application", mediatypeNode.ChildNode("type").Text);
            Assert.Equal("utf-8", charsetParameter.ChildNode("value").Text);
            Assert.Equal("false", schemaParameter.ChildNode("value").Text);
        }

        [Fact]
        public void StrongContentType()
        {

            var contentTypeHeader = ContentTypeHeaderValue.Parse("application/xml; charset= utf-8;schema =false ");

            Assert.Equal("application", contentTypeHeader.MediaType.Type);
            Assert.Equal("utf-8", contentTypeHeader.Parameters["charset"]);
            Assert.Equal("false", contentTypeHeader.Parameters["schema"]);
        }

        [Fact]
        public void GenerateUserAgent()
        {

            var userAgent = new UserAgentHeaderValue();
            userAgent.Products.Add(new Product() {Name = "foo", Version = "1.0"});
            var productWithComments = new Product() {Name = "bar"};
            productWithComments.Comments.Add("(test)");  // Parentheses should not be here.
            userAgent.Products.Add(productWithComments);
            Assert.Equal("foo/1.0 bar (test)", userAgent.ToString());
        }

        [Fact]
        public void UserAgentProductWithVersion()
        {
            var x = new Expression("productex")
            {
                new Token("product"),
                new OptionalExpression("versionex") {new Literal("/"), new Token("version")}
            };
            var node = x.Consume(new Inputdata("foo/1.0"));

            Assert.Equal("foo",node.ChildNode("product").Text);
            Assert.Equal("1.0", node.ChildNode("versionex").ChildNode("version").Text);
        }

        [Fact]
        public void UserAgentProductWithoutVersion()
        {
            var x = new Expression("productex")
            {
                new Token("product"),
                new OptionalExpression("versionex") {new Literal("/"), new Token("version")}
            };
            var node = x.Consume(new Inputdata("foo"));

            Assert.Equal("foo", node.ChildNode("product").Text);
            Assert.Equal("",node.ChildNode("versionex").Text);
            Assert.Equal(true, node.ChildNode("versionex").NotPresent);
            
        }

        [Fact]
        public void UserAgent()
        {

            var userAgent = new Expression("useragent")
            {
                new Token("product"),
                new OptionalExpression("versionex"){ new Literal("/"), new Token("version") },  //Optional version
                new Rws(),
                new DelimitedList("productorcomments", " ", new Expression("token")
                {
                    new Ows(),
                    new OrExpression("productorcomment") {
                            new Comment("comment"), 
                            new Expression("productex")
                            {
                                new Token("product"),
                                new OptionalExpression("versionex"){ new Literal("/"), new Token("version") }
                            }   
                    }
                })
            };

            var userAgentNode = userAgent.Consume(new Inputdata("Mozilla/4.0 (compatible; Linux 2.6.22) NetFront/3.4 Kindle/2.5 (screen 824x1200;rotate)"));

            var primaryProductNode = userAgentNode.ChildNode("product");
            var primaryVersionNode = userAgentNode.ChildNode("versionex");
            var productOrCommentsNode = userAgentNode.ChildNode("productorcomments");
            Assert.Equal("(compatible; Linux 2.6.22)", productOrCommentsNode.ChildNodes.First().ChildNode("comment").Text);
            Assert.Equal("Mozilla", primaryProductNode.Text);
            Assert.Equal("4.0", primaryVersionNode.ChildNode("version").Text);
            Assert.Equal(4,productOrCommentsNode.ChildNodes.Count);
            
        }


        [Fact]
        public void UserAgentStrongType()
        {

            var userAgent =
                UserAgentHeaderValue.Parse(
                    "Mozilla/4.0 (compatible; Linux 2.6.22) NetFront/3.4 Kindle/2.5 (screen 824x1200;rotate)");
            var primaryProduct = userAgent.Products.First();
            var lastProduct = userAgent.Products.Last();
            Assert.Equal("(compatible; Linux 2.6.22)", primaryProduct.Comments.First());
            Assert.Equal("(screen 824x1200;rotate)", lastProduct.Comments.First());

            Assert.Equal("Mozilla", primaryProduct.Name);
            Assert.Equal("4.0", primaryProduct.Version);

            Assert.Equal("Kindle", lastProduct.Name);
            Assert.Equal("2.5", lastProduct.Version);

            Assert.Equal(3, userAgent.Products.Count);

        }

        [Fact]
        public void AuthHeader()
        {
            var authHeader = new Expression("authorization")
            {
                new Token("scheme"),
                new Rws(),
                new Token("parameter")
            };

            var authHeaderNode = authHeader.Consume(new Inputdata("Basic asddasdasdasdasd"));

            Assert.Equal("Basic",authHeaderNode.ChildNode("scheme").Text);
            Assert.Equal("asddasdasdasdasd", authHeaderNode.ChildNode("parameter").Text);

        }

    }
}
