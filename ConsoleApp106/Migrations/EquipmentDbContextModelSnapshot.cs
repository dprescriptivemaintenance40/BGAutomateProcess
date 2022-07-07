﻿// <auto-generated />
using ConsoleApp106.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp106.Migrations
{
    [DbContext(typeof(EquipmentDbContext))]
    partial class EquipmentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+BatchTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DateTimeUploaded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(200)");

                    b.Property<int>("EquipmentProcessId")
                        .HasColumnType("int");

                    b.Property<int>("EquipmentTblId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Batch");
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+CleanTableCompressor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BatchId")
                        .HasColumnType("int");

                    b.Property<string>("DT1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DT2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Compressor_Cleaning");
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+ErrorTableCompressor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BatchId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("rowAffected")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Compressor_Error");
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+PredictedTableCompressor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BatchId")
                        .HasColumnType("int");

                    b.Property<string>("DT1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DT2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Compressor_Predicted");
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+ProcessedTableCompressor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BatchId")
                        .HasColumnType("int");

                    b.Property<string>("DT1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DT2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Compressor_Processed");
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+StagingTableCompressor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BatchId")
                        .HasColumnType("int");

                    b.Property<string>("DT1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DT2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PR2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TD2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TS2")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.ToTable("Compressor_Staging");
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+CleanTableCompressor", b =>
                {
                    b.HasOne("ConsoleApp106.Model.CompressorModel+BatchTable", "batchTable")
                        .WithMany()
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+ErrorTableCompressor", b =>
                {
                    b.HasOne("ConsoleApp106.Model.CompressorModel+BatchTable", "batchTable")
                        .WithMany()
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+PredictedTableCompressor", b =>
                {
                    b.HasOne("ConsoleApp106.Model.CompressorModel+BatchTable", "batchTable")
                        .WithMany()
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+ProcessedTableCompressor", b =>
                {
                    b.HasOne("ConsoleApp106.Model.CompressorModel+BatchTable", "batchTable")
                        .WithMany()
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ConsoleApp106.Model.CompressorModel+StagingTableCompressor", b =>
                {
                    b.HasOne("ConsoleApp106.Model.CompressorModel+BatchTable", "batchTable")
                        .WithMany()
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
