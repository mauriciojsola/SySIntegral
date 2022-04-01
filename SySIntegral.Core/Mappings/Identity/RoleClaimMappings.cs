using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class RoleClaimMappings : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> b)
        {
            b.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // Primary key
            b.HasKey(rc => rc.Id);

            b.Property(p => p.ClaimType)
                .HasColumnType("nvarchar(max)");

            b.Property<string>(p => p.ClaimValue)
                .HasColumnType("nvarchar(max)");

            b.Property(p => p.RoleId)
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            b.HasIndex(p => p.RoleId);

            // Maps to the AspNetRoleClaims table
            b.ToTable("AspNetRoleClaims");

        }
    }
}
