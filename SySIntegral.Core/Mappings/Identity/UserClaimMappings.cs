using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class UserClaimMappings : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> entity)
        {
            entity.Property(p => p.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            entity.Property(p => p.ClaimType)
                .HasColumnType("nvarchar(max)");

            entity.Property<string>(p => p.ClaimValue)
                .HasColumnType("nvarchar(max)");

            entity.Property(p => p.UserId)
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.UserId);

            entity.ToTable("AspNetUserClaims");
        }
    }
}
