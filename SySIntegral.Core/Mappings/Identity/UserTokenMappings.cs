using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class UserTokenMappings : IEntityTypeConfiguration<IdentityUserToken<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<string>> b)
        {
            b.Property(p => p.UserId).HasColumnType("nvarchar(450)");        

            b.Property(p => p.LoginProvider)
                .HasColumnType("nvarchar(128)")
                .HasMaxLength(128);

            b.Property(p => p.Name)
                .HasColumnType("nvarchar(128)")
                .HasMaxLength(128);

            b.Property(p => p.Value)
                .HasColumnType("nvarchar(max)");
            
            // Composite primary key consisting of the UserId, LoginProvider and Name
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            

            // Maps to the AspNetUserTokens table
            b.ToTable("AspNetUserTokens");
        }
    }
}
