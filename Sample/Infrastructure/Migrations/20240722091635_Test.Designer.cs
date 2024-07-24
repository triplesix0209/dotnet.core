﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sample.Infrastructure.DataContext;

#nullable disable

namespace Sample.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240722091635_Test")]
    partial class Test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sample.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Mã số");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2")
                        .HasComment("Thời gian khởi tạo");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Id người tạo");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("datetime2")
                        .HasComment("Thời gian xóa");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("Tên gọi");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Id chi nhánh");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2")
                        .HasComment("Thời gian chỉnh sửa cuối");

                    b.Property<Guid?>("UpdatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Id người chỉnh sửa");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CreateAt");

                    b.HasIndex("CreatorId");

                    b.HasIndex("DeleteAt");

                    b.HasIndex("Name");

                    b.HasIndex("SiteId");

                    b.HasIndex("UpdateAt");

                    b.HasIndex("UpdatorId");

                    b.ToTable("Account", t =>
                        {
                            t.HasComment("Tài khoản");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("653dc4d4-ca05-45ac-83cd-e98fa91b890f"),
                            Code = "EM001",
                            Name = "Nhân Viên 1",
                            SiteId = new Guid("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22")
                        },
                        new
                        {
                            Id = new Guid("6f6e615e-feeb-40b5-b53c-7f9056082d36"),
                            Code = "EM002",
                            Name = "Nhân Viên 2",
                            SiteId = new Guid("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22")
                        },
                        new
                        {
                            Id = new Guid("72b44a93-defc-4e24-a466-0d0d36b3669c"),
                            Code = "EM003",
                            Name = "Nhân Viên 3",
                            SiteId = new Guid("3e08cf2e-d8a2-49b5-8663-fa31f0cdd168")
                        });
                });

            modelBuilder.Entity("Sample.Domain.Entities.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Mã số");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2")
                        .HasComment("Thời gian khởi tạo");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Id người tạo");

                    b.Property<DateTime?>("DeleteAt")
                        .HasColumnType("datetime2")
                        .HasComment("Thời gian xóa");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("Tên gọi");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2")
                        .HasComment("Thời gian chỉnh sửa cuối");

                    b.Property<Guid?>("UpdatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Id người chỉnh sửa");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CreateAt");

                    b.HasIndex("CreatorId");

                    b.HasIndex("DeleteAt");

                    b.HasIndex("Name");

                    b.HasIndex("UpdateAt");

                    b.HasIndex("UpdatorId");

                    b.ToTable("Site", t =>
                        {
                            t.HasComment("Chi nhánh");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("7a2ed7c2-e6f7-48c1-a86a-aa701aee1e22"),
                            Code = "H001",
                            Name = "Quận 5"
                        },
                        new
                        {
                            Id = new Guid("3e08cf2e-d8a2-49b5-8663-fa31f0cdd168"),
                            Code = "H002",
                            Name = "Quận 6"
                        });
                });

            modelBuilder.Entity("Sample.Domain.Entities.Account", b =>
                {
                    b.HasOne("Sample.Domain.Entities.Site", "Site")
                        .WithMany("Accounts")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Site");
                });

            modelBuilder.Entity("Sample.Domain.Entities.Site", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
