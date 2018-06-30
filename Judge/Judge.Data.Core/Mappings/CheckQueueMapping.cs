using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class CheckQueueMapping : IEntityTypeConfiguration<CheckQueue>
    {
        public void Configure(EntityTypeBuilder<CheckQueue> builder)
        {
            builder.HasKey(o => o.SubmitResultId);
            builder.Property(o => o.CreationDateUtc).ValueGeneratedOnAdd();

            builder.ToTable("CheckQueue", "dbo");
        }
    }
}
