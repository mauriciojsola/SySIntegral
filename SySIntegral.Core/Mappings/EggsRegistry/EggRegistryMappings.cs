using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.EggsRegistry;

namespace SySIntegral.Core.Mappings.EggsRegistry
{
    public class EggRegistryMappings : IEntityTypeConfiguration<EggRegistry>
    {
        public void Configure(EntityTypeBuilder<EggRegistry> entity)
        {
            entity.Property(p => p.Id)
                .HasColumnType("int").UseIdentityColumn();
            
            entity.HasKey(k => k.Id);

            entity.Property(p => p.DeviceId)
                .HasColumnName("DeviceId")
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Timestamp)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(p => p.WhiteEggsCount)
                .HasColumnType("int") //SqlDbType.Int.ToString()
                .HasDefaultValue(0);

            entity.Property(p => p.ColorEggsCount)
                .HasColumnType("int")
                .HasDefaultValue(0);
                    
            entity.ToTable("EggRegistry");
            
        }
    }
}
