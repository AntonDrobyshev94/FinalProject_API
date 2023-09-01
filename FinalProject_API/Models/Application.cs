namespace FinalProject_API.Models
{
    public class Application: IApplication
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
