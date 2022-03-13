//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace SySIntegral.Core.Entities.Users
//{
//    public class PropertyMailingAddressConfig : IEntityTypeConfiguration<ApplicationUser>
//    {
//        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
//        {
//            //builder
//            //    .HasOne(d => d.LkAddressType);
//            //    .WithMany(p => p.PropertyMailingAddress);
//            //    .HasForeignKey(d => d.LkAddressTypeId)
//            //    .HasConstraintName("FK_PropertyMailingAddress_LK_AddressTypeId");

//            builder.ToTable("AspNetUsers");
//            builder.HasKey(x => x.Id);

//            //builder.Entity("SySIntegral.Core.Entities.Users.ApplicationUser", b =>
//            //    {
//            //        b.Property<string>("Id")
//            //            .HasColumnType("nvarchar(450)");

//            //        b.Property<int>("AccessFailedCount")
//            //            .HasColumnType("int");

//            //        b.Property<string>("ConcurrencyStamp")
//            //            .IsConcurrencyToken()
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<string>("Email")
//            //            .HasColumnType("nvarchar(256)")
//            //            .HasMaxLength(256);

//            //        b.Property<bool>("EmailConfirmed")
//            //            .HasColumnType("bit");

//            //        b.Property<string>("FirstName")
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<string>("LastName")
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<bool>("LockoutEnabled")
//            //            .HasColumnType("bit");

//            //        b.Property<DateTimeOffset?>("LockoutEnd")
//            //            .HasColumnType("datetimeoffset");

//            //        b.Property<string>("NormalizedEmail")
//            //            .HasColumnType("nvarchar(256)")
//            //            .HasMaxLength(256);

//            //        b.Property<string>("NormalizedUserName")
//            //            .HasColumnType("nvarchar(256)")
//            //            .HasMaxLength(256);

//            //        b.Property<string>("OrganizationId")
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<string>("PasswordHash")
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<string>("PhoneNumber")
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<bool>("PhoneNumberConfirmed")
//            //            .HasColumnType("bit");

//            //        b.Property<string>("SecurityStamp")
//            //            .HasColumnType("nvarchar(max)");

//            //        b.Property<bool>("TwoFactorEnabled")
//            //            .HasColumnType("bit");

//            //        b.Property<string>("UserName")
//            //            .HasColumnType("nvarchar(256)")
//            //            .HasMaxLength(256);

//            //        b.HasKey("Id");

//            //        b.HasIndex("NormalizedEmail")
//            //            .HasName("EmailIndex");

//            //        b.HasIndex("NormalizedUserName")
//            //            .IsUnique()
//            //            .HasName("UserNameIndex")
//            //            .HasFilter("[NormalizedUserName] IS NOT NULL");

//            //        b.ToTable("AspNetUsers");
//            //    });


//        }
//    }
//}
