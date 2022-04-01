using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Identity
{
    public class ApplicationUserMappings : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            // Primary key
            entity.HasKey(u => u.Id);

            // Indexes for "normalized" username and email, to allow efficient lookups
            entity.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique().HasFilter("[NormalizedUserName] IS NOT NULL");
            entity.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

            // Maps to the AspNetUsers table
            entity.ToTable("AspNetUsers");

            // A concurrency token for use with the optimistic concurrency checking
            entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken().HasColumnType("nvarchar(max)");

            // Limit the size of columns to use efficient database types
            entity.Property(u => u.UserName).HasMaxLength(256);
            entity.Property(u => u.NormalizedUserName).HasMaxLength(256);
            entity.Property(u => u.Email).HasMaxLength(256);
            entity.Property(u => u.NormalizedEmail).HasMaxLength(256);

            entity.Property(u => u.AccessFailedCount).HasColumnType("int");
            entity.Property(u => u.EmailConfirmed).HasColumnType("bit");
            entity.Property(u => u.FirstName).HasColumnType("nvarchar(max)");
            entity.Property(u => u.LastName).HasColumnType("nvarchar(max)");

            entity.Property(u => u.LockoutEnabled).HasColumnType("bit");
            entity.Property(u => u.LockoutEnd).HasColumnType("datetimeoffset");

            entity.Property(u => u.OrganizationId).HasColumnType("int").IsRequired();
            entity.HasIndex(u => u.OrganizationId).HasName("IX_AspNetUsers_OrganizationId");

            entity.Property(u => u.PasswordHash).HasColumnType("nvarchar(max)");

            entity.Property(u => u.PhoneNumber).HasColumnType("nvarchar(max)");
            entity.Property(u => u.PhoneNumberConfirmed).HasColumnType("bit");

            entity.Property(u => u.SecurityStamp).HasColumnType("nvarchar(max)");

            entity.Property(u => u.TwoFactorEnabled).HasColumnType("bit");

            // The relationships between User and other entity types
            // Note that these relationships are configured with no navigation properties

            //// Each User can have many UserClaims
            //b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            //// Each User can have many UserLogins
            //b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            //// Each User can have many UserTokens
            //b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            //// Each User can have many entries in the UserRole join table
            //b.HasMany<TUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                     

        }
    }
}
