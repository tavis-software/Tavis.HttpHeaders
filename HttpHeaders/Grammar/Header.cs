using System.Text;

namespace Headers
{
    public class Header
    {
        public string FieldName { get; set; }
        public IHeaderFieldValue FieldValue { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(FieldName);
            sb.Append(":");
            sb.Append(FieldValue.ToString());
            return sb.ToString();
        }

        public void Parse(string rawHeader)
        {
            var pair = rawHeader.Split(':');
            FieldName = pair[0];
            // Not sure how to parse FieldValue
        }
    }
}