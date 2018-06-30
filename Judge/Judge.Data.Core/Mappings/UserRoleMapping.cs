using Judge.Model.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Judge.Data.Core.Mappings
{
    internal sealed class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey("UserId");

            builder.ToTable("UserRoles");
        }
    }
}
