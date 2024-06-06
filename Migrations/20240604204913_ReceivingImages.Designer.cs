﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameOfThrones.Migrations
{
    [DbContext(typeof(GameOfThronesContext))]
    [Migration("20240604204913_ReceivingImages")]
    partial class ReceivingImages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CharacterId"));

                    b.Property<string>("Born")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DataId")
                        .HasColumnType("integer");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("House")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("Titles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("CharacterId");

                    b.HasIndex("DataId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Data", b =>
                {
                    b.Property<int>("DataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DataId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DataId");

                    b.ToTable("Datas");
                });

            modelBuilder.Entity("Dragon", b =>
                {
                    b.Property<int>("DragonId")
                        .HasColumnType("integer");

                    b.Property<int>("DataId")
                        .HasColumnType("integer");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DragonId");

                    b.ToTable("Dragons");
                });

            modelBuilder.Entity("House", b =>
                {
                    b.Property<int>("HouseId")
                        .HasColumnType("integer");

                    b.Property<int>("DataId")
                        .HasColumnType("integer");

                    b.Property<string>("HouseName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lord")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("HouseId");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("Character", b =>
                {
                    b.HasOne("Data", "Data")
                        .WithMany()
                        .HasForeignKey("DataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Data");
                });

            modelBuilder.Entity("Dragon", b =>
                {
                    b.HasOne("Data", "Data")
                        .WithMany()
                        .HasForeignKey("DragonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Data");
                });

            modelBuilder.Entity("House", b =>
                {
                    b.HasOne("Data", "Data")
                        .WithMany()
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Data");
                });
#pragma warning restore 612, 618
        }
    }
}