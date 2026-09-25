using finalfinalfinal.Models; 

namespace finalfinalfinal.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
