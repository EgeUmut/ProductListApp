namespace ProductListApp.Models
{
    public class Message
    {
        public int id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string UserMessage { get; set; }
        public bool Read { get; set; }
    }
}
