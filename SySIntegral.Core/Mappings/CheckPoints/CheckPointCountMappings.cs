using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Devices;

namespace SySIntegral.Core.Mappings.CheckPoints
{
    public class CheckPointCountMappings : IEntityTypeConfiguration<CheckPointCount>
    {
        public void Configure(EntityTypeBuilder<CheckPointCount> entity)
        {
            // Maps to the corresponding table
            entity.ToTable("CheckPointCount");

            entity.HasKey(bc => new { CheckPointId = bc.CheckPointId, bc.EggRegistryId});  

            entity.HasOne(bc => bc.CheckPoint)
                .WithMany(b => b.Countings)
                .HasForeignKey(bc => bc.CheckPointId);  
            

        }
    }
}
