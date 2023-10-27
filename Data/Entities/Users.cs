using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Password { get; set; }
    public string RoleType { get; set; }
}
