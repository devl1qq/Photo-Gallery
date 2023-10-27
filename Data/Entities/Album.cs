namespace Data.Entities;
public class Album
{
public Guid Id { get; set; }
public Guid CreatedByUserId { get; set; }
public string Name { get; set; }
public string Path { get; set; }
public DateTime CreatedAt { get; set; }
}

