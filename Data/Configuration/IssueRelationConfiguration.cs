using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleTaskTracker.Models;

namespace SimpleTaskTracker.Data.Configuration;

public class IssueRelationConfiguration : IEntityTypeConfiguration<IssueRelation>
{
    public void Configure(EntityTypeBuilder<IssueRelation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.RelationType)
                .HasConversion<string>()
                .IsRequired();

        builder.HasOne(r => r.FromIssue)
                .WithMany(i => i.RelationsFrom)
                .HasForeignKey(r => r.FromIssueId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ToIssue)
                .WithMany(i => i.RelationsTo)
                .HasForeignKey(r => r.ToIssueId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new IssueRelation
            {
                Id = 1,
                FromIssueId = 2,
                ToIssueId = 3,
                RelationType = RelationType.Block
            },
            new IssueRelation
            {
                Id = 2,
                FromIssueId = 1,
                ToIssueId = 5,
                RelationType = RelationType.Block
            }
        );

    }
}
