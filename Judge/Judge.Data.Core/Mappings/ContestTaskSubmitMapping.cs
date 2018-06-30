using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class ContestTaskSubmitMapping : IEntityTypeConfiguration<ContestTaskSubmit>
    {
        private const byte SubmitType = 2;

        public void Configure(EntityTypeBuilder<ContestTaskSubmit> builder)
        {
            builder.HasDiscriminator<byte>(@"SubmitType").HasValue(SubmitType);
        }
    }
}
