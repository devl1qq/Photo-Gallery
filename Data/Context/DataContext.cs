using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DataContext() : base() { }

    public DbSet<User> Users { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<UserPictureInteraction> Interactions { get; set; }
    public DbSet<Album> Albums { get; set; }
}

