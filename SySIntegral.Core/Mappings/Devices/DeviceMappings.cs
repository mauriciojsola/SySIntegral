using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.Devices;

namespace SySIntegral.Core.Mappings.Devices
{
    public class DeviceMappings : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> entity)
        {
            // Maps to the corresponding table
            entity.ToTable("Device");

            entity.Property(p => p.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn); //.UseIdentityColumn();

            entity.HasKey(u => u.Id);

            entity.HasDiscriminator(b => b.DeviceType);
            entity.HasDiscriminator<DeviceType>("DeviceType")
                .HasValue<CollectorDevice>(DeviceType.Collector)
                .HasValue<LineDevice>(DeviceType.Line)
                .HasValue<CounterDevice>(DeviceType.Counter);

            entity.Property(e => e.DeviceType)
                .HasColumnName("DeviceType")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.Description)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.UniqueId)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.AssetId)
                .HasColumnType("int")
                .IsRequired();

            entity.HasIndex(u => u.UniqueId)
                .HasName("IX_UniqueId");

            entity.HasOne<Asset>(x => x.Asset)
                .WithMany(x => x.Devices)
                .HasForeignKey(x => x.AssetId);

        }
    }
}
