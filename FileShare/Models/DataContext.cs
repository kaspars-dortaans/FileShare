namespace FileShare.Models;

using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Setting> Settings { get; set; }

    public DataContext(DbContextOptions<DataContext> options): base(options)
    { }
}