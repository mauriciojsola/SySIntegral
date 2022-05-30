using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Devices;
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

            //entity.Property(p => p.OldDeviceId)
            //    .HasColumnName("OldDeviceId")
            //    .HasColumnType("nvarchar(100)")
            //    .HasMaxLength(100)
            //    .IsRequired();

            entity.Property(u => u.DeviceId)
                .HasColumnType("int")
                .IsRequired();

            entity.HasIndex(u => u.DeviceId)
                .HasName("IX_DeviceId");

            entity.Property(p => p.Timestamp)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(p => p.WhiteEggsCount)
                .HasColumnType("int") //SqlDbType.Int.ToString()
                .HasDefaultValue(0);

            entity.Property(p => p.ColorEggsCount)
                .HasColumnType("int")
                .HasDefaultValue(0);

            entity.Property(p => p.ReadTimestamp)
                .HasColumnType("datetime2");

            entity.Property(p => p.ExportTimestamp)
                .HasColumnType("datetime2");

            entity.HasOne<Device>(x => x.Device);

            entity.ToTable("EggRegistry");
            
        }
    }
}
