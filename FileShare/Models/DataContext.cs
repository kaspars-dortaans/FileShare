namespace FileShare.Models;

using FileShare.Enums;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Setting> Settings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //Development data seeding
        builder.Entity<User>().HasData(new User() { Id = 1, FirstName = "Admin", LastName = "Test", Role = Role.Admin, Username = "Admin", PasswordHash = "$2a$11$Mprpo/PxbtIDUx/UIhX89e1L2GgPEiu0w4yZ5KDor1ECPaZRE789O" });
        //Password: "Test123"
    }

    public DataContext(DbContextOptions<DataContext> options): base(options)
    { }
}