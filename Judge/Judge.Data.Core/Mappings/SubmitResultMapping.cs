using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class SubmitResultMapping : IEntityTypeConfiguration<SubmitResult>
    {
        public void Configure(EntityTypeBuilder<SubmitResult> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.HasOne(o => o.CheckQueue)
                .WithOne();

            builder.ToTable("SubmitResults", "dbo");
        }
    }
}
