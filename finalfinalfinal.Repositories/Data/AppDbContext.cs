using Microsoft.EntityFrameworkCore;
using finalfinalfinal.Models;

namespace finalfinalfinal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

        mb.Entity<TaskItem>()
          .HasOne(t => t.Project)
          .WithMany(p => p.Tasks)
          .HasForeignKey(t => t.ProjectId)
          .OnDelete(DeleteBehavior.SetNull);
    }
}