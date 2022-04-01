using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SySIntegral.Core.Entities.Organizations;

namespace SySIntegral.Core.Mappings.Organizations
{
    public class OrganizationMappings : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> entity)
        {
            entity.Property(p => p.Id)
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn); //.UseIdentityColumn();
            
            entity.Property(p => p.Name)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasKey(k => k.Id);

            entity.ToTable("Organization");


        }
    }
}
