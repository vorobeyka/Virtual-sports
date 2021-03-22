﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VirtualSports.BE.Contexts;

namespace VirtualSports.BE.Migrations
{
    [DbContext(typeof(DatabaseManagerContext))]
    partial class DatabaseManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("VirtualSports.BE.Models.DatabaseModels.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("VirtualSports.BE.Models.DatabaseModels.Game", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<List<string>>("Categories")
                        .HasColumnType("text[]");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Provider")
                        .HasColumnType("text");

                    b.Property<List<string>>("Tags")
                        .HasColumnType("text[]");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("VirtualSports.BE.Models.DatabaseModels.Provider", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("VirtualSports.BE.Models.DatabaseModels.Tag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("VirtualSports.BE.Models.DatabaseModels.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("FavouriteGameIds")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("FavouriteGameMobileIds")
                        .HasColumnType("text[]");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<List<string>>("RecentGameIds")
                        .HasColumnType("text[]");

                    b.Property<List<string>>("RecentMobileGameIds")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
