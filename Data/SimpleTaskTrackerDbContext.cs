using Microsoft.EntityFrameworkCore;
using SimpleTaskTracker.Models;
using SimpleTaskTracker.Data.Configuration;

namespace SimpleTaskTracker.Data;

public class SimpleTaskTrackerDbContext : DbContext
{
    public DbSet<IssueItem> IssueItems { get; set; }
    public DbSet<IssueRelation> IssueRelations { get; set; }
    public SimpleTaskTrackerDbContext()
    {
        Database.EnsureCreated();
    }
    public SimpleTaskTrackerDbContext(DbContextOptions<SimpleTaskTrackerDbContext> options)
            : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IssueItemConfiguration());
        modelBuilder.ApplyConfiguration(new IssueRelationConfiguration());
    }
}