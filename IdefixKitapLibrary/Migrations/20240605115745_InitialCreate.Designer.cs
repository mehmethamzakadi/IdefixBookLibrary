﻿// <auto-generated />
using IdefixKitapLibrary.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IdefixKitapLibrary.Migrations
{
    [DbContext(typeof(BookLibraryContext))]
    [Migration("20240605115745_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IdefixKitapLibrary.Models.Kitap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BasimDili")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<int>("ImageHeight")
                        .HasColumnType("integer");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<int>("ImageWidth")
                        .HasColumnType("integer");

                    b.Property<string>("Kategori")
                        .HasColumnType("text");

                    b.Property<string>("KitapAdi")
                        .HasColumnType("text");

                    b.Property<string>("YayinEvi")
                        .HasColumnType("text");

                    b.Property<string>("YazarAdi")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Kitaplar");
                });
#pragma warning restore 612, 618
        }
    }
}
