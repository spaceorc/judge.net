using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class ProblemSubmitMapping : IEntityTypeConfiguration<ProblemSubmit>
    {
        private const byte SubmitType = 1;

        public void Configure(EntityTypeBuilder<ProblemSubmit> builder)
        {
            builder.HasDiscriminator<byte>(@"SubmitType").HasValue(SubmitType);
        }
    }
}
