namespace Tavis.Headers
{
    public class MediaType
    {
        public string Type { get; set; }
        public string SubType { get; set; }

        public override string ToString()
        {
            return Type + "/" + SubType;
        }
    }
}