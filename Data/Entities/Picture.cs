using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities;

public class Picture
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public string Path { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
