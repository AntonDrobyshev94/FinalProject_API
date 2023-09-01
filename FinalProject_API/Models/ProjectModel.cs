namespace FinalProject_API.Models
{
    public class ProjectModel : IProjectModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
