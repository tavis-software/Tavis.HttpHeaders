using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tavis.Headers;
using Tavis.Headers.Elements;
using Tavis.Parser;
using Xunit;
using Xunit.Extensions;

namespace Tavis.HeadersTests
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
                MediaType.Syntax,
                new Literal(";"),
                new SemiColonList("parameters",  Parameter.Syntax)
            };

            var contentTypeNode = contentType.Consume(new Inputdata("application/xml;charset=utf-8;schema=false"));

            var mediatypeNode = contentTypeNode.ChildNode("mediatype");
            var parametersNode = contentTypeNode.ChildNode("parameters");
            var charsetParameter = parametersNode.ChildNodeContains("name", "charset");
            var schemaParameter = parametersNode.ChildNodeContains("name", "schema");


            Assert.NotNull(mediatypeNode);
            Assert.NotNull(parametersNode);
            Assert.Equal("application", mediatypeNode.ChildNode("type").Text);
            Assert.Equal("utf-8", charsetParameter.ChildNode("tokenvalue").Text);
            Assert.Equal("false", schemaParameter.ChildNode("tokenvalue").Text);
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
            Assert.Equal(false, node.ChildNode("versionex").Present);
            
        }

        [Fact]
        public void UserAgent()
        {

            var userAgent = new Expression("useragent")
            {
                new Token("product"),
                new OptionalExpression("versionex"){ new Literal("/"), new Token("version") },  //Optional version
                new Rws(),
                new DelimitedList("productorcomments", " ", 
                    new OrExpression("productorcomment") {
                            new Comment("comment"), 
                            new Expression("productex")
                            {
                                new Token("product"),
                                new OptionalExpression("versionex"){ new Literal("/"), new Token("version") }
                            }   
                    
                })
            };

            var userAgentNode = userAgent.Consume(new Inputdata("Mozilla/4.0 (compatible; Linux 2.6.22) NetFront/3.4 Kindle/2.5 (screen 824x1200;rotate)"));

            var primaryProductNode = userAgentNode.ChildNode("product");
            var primaryVersionNode = userAgentNode.ChildNode("versionex");
            var productOrCommentsNode = userAgentNode.ChildNode("productorcomments");
            Assert.Equal("compatible; Linux 2.6.22", productOrCommentsNode.ChildNodes.First().Text);
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
            Assert.Equal("compatible; Linux 2.6.22", primaryProduct.Comments.First());
            Assert.Equal("screen 824x1200;rotate", lastProduct.Comments.First());

            Assert.Equal("Mozilla", primaryProduct.Name);
            Assert.Equal("4.0", primaryProduct.Version);

            Assert.Equal("Kindle", lastProduct.Name);
            Assert.Equal("2.5", lastProduct.Version);

            Assert.Equal(3, userAgent.Products.Count);

        }

        [Fact]
        public void AuthHeader()
        {

            var authHeaderNode = AuthorizationHeaderValue.Parse("Basic asddasdasdasdasd==", useToken68: true);

            Assert.Equal("Basic",authHeaderNode.Scheme);
            Assert.Equal("asddasdasdasdasd==", authHeaderNode.Token);
            Assert.Equal(0, authHeaderNode.Errors.Count);
        }

        [Fact]
        public void AuthHeaderWithParameters()
        {

            var authHeaderNode = AuthorizationHeaderValue.Parse("Basic foo=bar,foo=\"ba,z\",yo=blah");

            Assert.Equal("Basic", authHeaderNode.Scheme);
            Assert.Equal(3, authHeaderNode.Parameters.Count);
            Assert.Equal("yo", authHeaderNode.Parameters[2].Name);
            Assert.Equal("blah", authHeaderNode.Parameters[2].Value);
            Assert.Equal("ba,z", authHeaderNode.Parameters[1].Value);
            Assert.Equal(0, authHeaderNode.Errors.Count);
        }

        [Fact]
        public void AuthHeaderWithoutParameter()
        {

            var authHeaderNode = AuthorizationHeaderValue.Parse("Magic");

            Assert.Equal("Magic", authHeaderNode.Scheme);
            Assert.Equal(null, authHeaderNode.Parameters);
            Assert.Equal(0, authHeaderNode.Errors.Count);
        }
        [Fact]
        public void AuthHeader_failure_missing_scheme()
        {
            var authHeaderNode = AuthorizationHeaderValue.Parse("foo=bar;test=boo");


            Assert.Equal("foo", authHeaderNode.Scheme);
            Assert.Null(authHeaderNode.Parameters);
            Assert.Equal(1,authHeaderNode.Errors.Count);
        }
        [Fact]
        public void UnrelatedArbitraryCommandPhrase()
        {
            var x = new Expression("cmd")
            {
                new Ows(),
                new Literal("runscope:"),
                new Ows(),
                new BasicRule("command", BasicRule.Char)
            };

            var parsedNode = x.Consume(new Inputdata("runscope:blah blah"));

            Assert.Equal("blah blah", parsedNode.ChildNode("command").Text);
        }

        //public static IExpression CommandSyntax = new Expression("cmd")
        //    {
        //        new Ows(),
        //        new Literal("runscope:"),
        //        new Ows(),
        //        new Headers.Token("verb"),
        //        new Rws(),
        //        new Headers.Token("bucket"),
        //        new Ows(),
        //        new OptionalExpression("testphrase")
        //        {
        //            new Literal("/"),
        //            new Ows(),
        //            new Headers.Token("test")
        //        },
        //        new OptionalExpression("paramlist")
        //        {
        //            new Rws(),
        //            new Literal("with"),
        //            new CommaList("parameters",Parameter.Syntax)    
        //        }
                
        //    };

        public static IExpression CommandSyntax = new Expression("cmd")
            {
                new Ows(),
                new Literal("runscope:"),
                new Ows(),
                new Headers.Token("verb"),
                new Rws(),
                new Headers.QuotedString("bucket"),
                new Ows(),
                new OptionalExpression("testphrase")
                {
                    new Literal("/"),
                    new Headers.QuotedString("test")
                },
                new Ows(),
                new OptionalExpression("paramlist")
                {
                    new Literal("with"),
                    new CommaList("parameters",Parameter.Syntax)    
                }
                
            };


        [Fact]
        public void UnrelatedArbitraryCommandPhrase2()
        {
            

            var parsedNode = CommandSyntax.Consume(new Inputdata("runscope: run  \"mybucket\" with domain = prod"));

            Assert.Equal("run", parsedNode.ChildNode("verb").Text);
            Assert.Equal("mybucket", parsedNode.ChildNode("bucket").Text);
            var paramlist = parsedNode.ChildNode("paramlist");
            Assert.NotNull( paramlist);
            
            var parameter = Parameter.Create(paramlist.ChildNode("parameters").ChildNodes.First());
            Assert.Equal("domain", parameter.Name);
            Assert.Equal("prod", parameter.Value);
            
        }
        [Fact]
        public void CommandPhraseWithMultipleParameters()
        {


            var parsedNode = CommandSyntax.Consume(new Inputdata("runscope: run  \"mybucket\" with domain = prod , x= y"));

            Assert.Equal("run", parsedNode.ChildNode("verb").Text);
            Assert.Equal("mybucket", parsedNode.ChildNode("bucket").Text);
            var paramlist = parsedNode.ChildNode("paramlist");
            Assert.NotNull(paramlist);

            var parameter = Parameter.Create(paramlist.ChildNode("parameters").ChildNodes.First());
            Assert.Equal("domain", parameter.Name);
            Assert.Equal("prod", parameter.Value);
            var parameter2 = Parameter.Create(paramlist.ChildNode("parameters").ChildNodes.Last());
            Assert.Equal("x", parameter2.Name);
            Assert.Equal("y", parameter2.Value);

        }


        [Fact]
        public void BenchmarkCommandPhrase()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                var parsedNode = CommandSyntax.Consume(new Inputdata("runscope: run  \"mybucket\"/\"test\" with domain = prod , x= y"));    
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed time {0:D}", stopwatch.ElapsedMilliseconds);

            
        }

        [Fact]
        public void CommandPhraseWithNoParameters()
        {


            var parsedNode = CommandSyntax.Consume(new Inputdata("runscope: run  \"mybucket\""));

            Assert.Equal("run", parsedNode.ChildNode("verb").Text);
            Assert.Equal("mybucket", parsedNode.ChildNode("bucket").Text);
            var paramlist = parsedNode.ChildNode("paramlist");
            Assert.Null(paramlist.ChildNodes);

        }

        [Theory,
        InlineData("runscope: run  \"mybucket\"/\"mytest\"")
        ]
        public void CommandPhraseWithBucketAndTest(string command)
        {

            var parsedNode = CommandSyntax.Consume(new Inputdata(command));

            Assert.Equal("run", parsedNode.ChildNode("verb").Text);
            Assert.Equal("mybucket", parsedNode.ChildNode("bucket").Text);
            var testphrase = parsedNode.ChildNode("testphrase");
            Assert.Equal("mytest", testphrase.ChildNode("test").Text);
            var paramlist = parsedNode.ChildNode("paramlist");
            Assert.Null(paramlist.ChildNodes);

        }

        [Fact]
        public void SingleParameter()
        {

            var parsedNode = Parameter.Syntax.Consume(new Inputdata("r = y"));

            var parameter = Parameter.Create(parsedNode);
            Assert.Equal("r", parameter.Name);
            Assert.Equal("y", parameter.Value);

        }

    }
}
