namespace TodoRedis.Core.Entities;
public class ToDo
{

    public ToDo(int id, string title, string description)
    {
        this.Id = id;
        Title = title;
        Description = description;
    }

    public int Id { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Done { get; set;} = false;
}
