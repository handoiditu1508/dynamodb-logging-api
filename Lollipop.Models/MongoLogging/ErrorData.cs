namespace Lollipop.Models.MongoLogging
{
    public class ErrorData
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
    }
}
