﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sample.Infrastructure.DataContext;

#nullable disable

namespace Sample.Infrastructure.Migrations.DataDb
{
    [DbContext(typeof(DataDbContext))]
    [Migration("20241224074610_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sample.Domain.Entities.Test", b =>
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

                    b.ToTable("Test", t =>
                        {
                            t.HasComment("Test");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
