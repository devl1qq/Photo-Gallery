using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities;

public class UserPictureInteraction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PictureId { get; set; }
    public bool IsLiked { get; set; }
    public bool IsDisliked { get; set; }
    public DateTime InteractionTime { get; set; }
}
