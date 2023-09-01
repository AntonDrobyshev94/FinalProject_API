namespace FinalProject_API.Models
{
    public interface IApplication
    {
        int ID { get; set; }
        string Name { get; set; }
        string EMail { get; set; }
        string Message { get; set; }
        string Status { get; set; }
        DateTime Date { get; set; }
    }
}
