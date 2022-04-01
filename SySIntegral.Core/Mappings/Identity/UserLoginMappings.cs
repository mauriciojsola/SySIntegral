using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class UserLoginMappings : IEntityTypeConfiguration<IdentityUserLogin<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> entity)
        {
            // Composite primary key consisting of the LoginProvider and the key to use
            // with that provider
            entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Limit the size of the composite key columns due to common DB restrictions
            entity.Property(l => l.LoginProvider).HasColumnType("nvarchar(128)").HasMaxLength(128);
            entity.Property(l => l.ProviderKey).HasColumnType("nvarchar(128)").HasMaxLength(128);

            entity.Property(p => p.ProviderDisplayName)
                .HasColumnType("nvarchar(max)");

            entity.Property(p => p.UserId)
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            //entity.HasKey("LoginProvider", "ProviderKey");

            entity.HasIndex(p=>p.UserId);

            entity.ToTable("AspNetUserLogins");
        }
    }
}
