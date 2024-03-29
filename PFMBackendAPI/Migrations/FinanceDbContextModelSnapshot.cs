﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PFMBackendAPI.Database;

#nullable disable

namespace PFMBackendAPI.Migrations
{
    [DbContext(typeof(FinanceDbContext))]
    partial class FinanceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ParentCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.SplitEntity", b =>
                {
                    b.Property<int>("SplitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SplitId"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("CatCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer");

                    b.HasKey("SplitId");

                    b.HasIndex("TransactionId");

                    b.ToTable("splits", (string)null);
                });

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransactionId"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("BeneficiaryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CatCode")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<char>("Direction")
                        .HasColumnType("character(1)");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Mcc")
                        .HasColumnType("integer");

                    b.HasKey("TransactionId");

                    b.HasIndex("CatCode");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.SplitEntity", b =>
                {
                    b.HasOne("PFMBackendAPI.Database.Entities.TransactionEntity", "Transaction")
                        .WithMany("Splits")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.HasOne("PFMBackendAPI.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CatCode");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("PFMBackendAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Navigation("Splits");
                });
#pragma warning restore 612, 618
        }
    }
}
