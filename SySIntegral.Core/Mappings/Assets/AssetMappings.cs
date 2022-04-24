using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;
using SySIntegral.Core.Entities.Users;

namespace SySIntegral.Core.Mappings.Assets
{
    public class AssetMappings : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> entity)
        {
            // Maps to the corresponding table
            entity.ToTable("Asset");

            entity.Property(p => p.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn); //.UseIdentityColumn();
            
            entity.HasKey(u => u.Id);
            
            entity.Property(p => p.Name)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(u => u.OrganizationId)
                .HasColumnType("int")
                .IsRequired();

            entity.HasIndex(u => u.OrganizationId)
                .HasName("IX_OrganizationId");

            entity.HasMany<Device>(x => x.Devices)
                .WithOne(x => x.Asset).HasForeignKey(x => x.AssetId).IsRequired();

        }
    }
}
