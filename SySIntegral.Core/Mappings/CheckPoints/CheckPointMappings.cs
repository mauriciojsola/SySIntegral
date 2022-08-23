using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Assets;
using SySIntegral.Core.Entities.CheckPoints;
using SySIntegral.Core.Entities.Devices;

namespace SySIntegral.Core.Mappings.CheckPoints
{
    public class CheckPointMappings : IEntityTypeConfiguration<CheckPoint>
    {
        public void Configure(EntityTypeBuilder<CheckPoint> entity)
        {
            // Maps to the corresponding table
            entity.ToTable("CheckPoint");

            entity.Property(p => p.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn); //.UseIdentityColumn();

            entity.HasKey(u => u.Id);

            entity.HasDiscriminator(b => b.CheckPointType);
            entity.HasDiscriminator<CheckPointType>("CheckPointType")
                .HasValue<AggregatorCheckPoint>(CheckPointType.Aggregator)
                .HasValue<LineCheckPoint>(CheckPointType.Line)
                .HasValue<SimpleCheckPoint>(CheckPointType.Simple)
                .HasValue<CheckPoint>(CheckPointType.Line);

            entity.Property(e => e.CheckPointType)
                .HasColumnName("CheckPointType")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(p => p.Description)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(u => u.AssetId)
                 .HasColumnType("int")
                 .IsRequired();
            entity.HasOne<Asset>(x => x.Asset);

            entity.Property(u => u.ParentId)
                .HasColumnType("int")
                .IsRequired(false);

            entity.HasOne(x => x.Parent);

            entity.HasMany(c => c.Children)
                .WithOne(p => p.Parent)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(u => u.InputDeviceId)
                .HasColumnName("InputDeviceId")
                .HasColumnType("int")
                .IsRequired(false);
            entity.HasOne<InputDevice>(x => x.InputDevice);

        }
    }
}
