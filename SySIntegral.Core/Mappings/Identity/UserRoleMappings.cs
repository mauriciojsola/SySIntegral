using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class UserRoleMappings : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> b)
        {
            b.Property(p => p.UserId).HasColumnType("nvarchar(450)");
            b.Property(p => p.RoleId).HasColumnType("nvarchar(450)");

            // Primary key
            b.HasKey(r => new { r.UserId, r.RoleId });

            b.HasIndex(p => p.RoleId).HasName("IX_AspNetUserRoles_RoleId");

            // Maps to the AspNetUserRoles table
            b.ToTable("AspNetUserRoles");


        }
    }
}
