using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class SubmitBaseMapping : IEntityTypeConfiguration<SubmitBase>
    {
        public void Configure(EntityTypeBuilder<SubmitBase> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();
            builder.Property(o => o.SubmitDateUtc).ValueGeneratedOnAdd();

            builder.HasMany(o => o.Results)
                .WithOne(o => o.Submit)
                .HasForeignKey("SubmitId");

            builder.ToTable("Submits", "dbo");
        }
    }
}
