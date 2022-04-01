using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class RoleMappings : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> entity)
        {
            entity.Property(u => u.Id).HasColumnType("nvarchar(450)").HasMaxLength(450);

            // Primary key
            entity.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            entity.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique().HasFilter("[NormalizedName] IS NOT NULL");

            // Maps to the AspNetRoles table
            entity.ToTable("AspNetRoles");

            // A concurrency token for use with the optimistic concurrency checking
            entity.Property(r => r.ConcurrencyStamp).HasColumnType("nvarchar(max)").IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            entity.Property(u => u.Name).HasColumnType("nvarchar(256)").HasMaxLength(256);
            entity.Property(u => u.NormalizedName).HasColumnType("nvarchar(256)").HasMaxLength(256);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            //entity.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            // Each Role can have many associated RoleClaims
            //entity.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

        }
    }
}
