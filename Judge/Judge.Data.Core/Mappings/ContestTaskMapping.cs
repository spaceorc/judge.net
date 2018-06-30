using Judge.Model.Core.Contests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class ContestTaskMapping : IEntityTypeConfiguration<ContestTask>
    {
        public void Configure(EntityTypeBuilder<ContestTask> builder)
        {
            builder.HasKey(o => new { o.TaskName });

            builder.HasOne(o => o.Task)
                .WithMany()
                .HasForeignKey(o => o.TaskId);

            builder.HasOne(o => o.Contest)
                .WithMany()
                .HasForeignKey("ContestId");

            builder.ToTable("ContestTasks");
        }
    }
}