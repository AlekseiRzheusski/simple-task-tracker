using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SimpleTaskTracker.Models;

namespace SimpleTaskTracker.Data.Configuration;

public class IssueItemConfiguration: IEntityTypeConfiguration<IssueItem>
{
    public void Configure(EntityTypeBuilder<IssueItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        builder.Property(i => i.Title).IsRequired();
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("DATETIME('now')");

        builder.HasData(
            new IssueItem
            {
                Id = 1,
                Title = "Add browse logic",
                Description = "Add method to show all issue information"
            },
            new IssueItem
            {
                Id = 2,
                Title = "Change the database provider",
                Description = "Change database provider to Microsoft.EntityFrameworkCore.SqlServer"
            },
            new IssueItem
            {
                Id = 3,
                Title = "Move connection string to file",
                Description = "Move MSSQL connection string with creds to separate file"
            },

            new IssueItem
            {
                Id = 4,
                Title = "Add authorisation and roles",
                Description = "Add authorisation and roles"
            },
            new IssueItem
            {
                Id = 5,
                Title = "Add list view",
                Description = "Add method to show all of the tasks"
            }
        );

    }
}
