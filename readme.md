# Tavis.HttpHeaders

This library implements the grammar used to define HTTP headers in RFC 7230-7235.  This grammar is then used to create strongly typed implementations of the standard HTTP headers.

This [blog post](http://www.bizcoder.com/everything-you-need-to-know-about-http-header-syntax-but-were-afraid-to-ask) provides and introduction, motivation and requirements for this project.

The syntax rules for a header can be defined as follows:

            var authHeader = new Expression("authorization")
            {
                new Token("scheme"),
                new Rws(),
                new Token("parameter")
            };

The grammar primitive objects encapsulate the syntax rules as defined by the HTTP specification.

The syntax rule can be used to parse a raw header like this,

            var authHeaderNode = authHeader.Consume(new Inputdata("Basic asddasdasdasdasd"));

            Assert.Equal("Basic",authHeaderNode.ChildNode("scheme").Text);
            Assert.Equal("asddasdasdasdasd", authHeaderNode.ChildNode("parameter").Text);

A more complex example would be the syntax for the user agent header.

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

These syntax rules can be encapsulated inside some strong types that then make it easy to access the headers.

Here is a test showing how to use the strong type that uses that syntax tree.

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

Currently only a few of the standard headers are implemented.  More will be coming soon.

This library is a PCL based library and so will work on Windows 8, Windows Phone 7.5, .Net 4.

A nuget package will be released soon.