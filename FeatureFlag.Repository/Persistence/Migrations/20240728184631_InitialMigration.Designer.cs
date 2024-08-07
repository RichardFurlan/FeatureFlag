﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Repository.Persistence;

#nullable disable

namespace Repository.Persistence.Migrations
{
    [DbContext(typeof(FeatureFlagDbContext))]
    [Migration("20240728184631_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FeatureFlag.Domain.Entities.Consumidor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Identificacao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("Inativo")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Consumidores");
                });

            modelBuilder.Entity("FeatureFlag.Domain.Entities.Recurso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Identificacao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("Inativo")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Recursos");
                });

            modelBuilder.Entity("FeatureFlag.Domain.Entities.RecursoConsumidor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CodigoConsumidor")
                        .HasColumnType("integer");

                    b.Property<int>("CodigoRecurso")
                        .HasColumnType("integer");

                    b.Property<int?>("ConsumidoresId")
                        .HasColumnType("integer");

                    b.Property<bool>("Inativo")
                        .HasColumnType("boolean");

                    b.Property<int?>("RecursosId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ConsumidoresId");

                    b.HasIndex("RecursosId");

                    b.ToTable("RecursosConsumidores");
                });

            modelBuilder.Entity("FeatureFlag.Domain.Entities.RecursoConsumidor", b =>
                {
                    b.HasOne("FeatureFlag.Domain.Entities.Consumidor", null)
                        .WithMany("RecursoConsumidores")
                        .HasForeignKey("ConsumidoresId");

                    b.HasOne("FeatureFlag.Domain.Entities.Recurso", null)
                        .WithMany("RecursoConsumidores")
                        .HasForeignKey("RecursosId");
                });

            modelBuilder.Entity("FeatureFlag.Domain.Entities.Consumidor", b =>
                {
                    b.Navigation("RecursoConsumidores");
                });

            modelBuilder.Entity("FeatureFlag.Domain.Entities.Recurso", b =>
                {
                    b.Navigation("RecursoConsumidores");
                });
#pragma warning restore 612, 618
        }
    }
}
