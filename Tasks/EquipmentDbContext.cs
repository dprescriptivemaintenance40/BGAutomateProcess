using Microsoft.EntityFrameworkCore;
using ConsoleApp106.Model;
namespace ConsoleApp106.DAL
{
    public class PlantDBContext : DbContext
    {
        public DbSet<BatchTable> BatchTables { get; set; }
        public DbSet<StagingTableCompressor> StagingTableSingles { get; set; }
        public DbSet<CleanTableCompressor> CleanTableSingles { get; set; }
        public DbSet<ErrorTableCompressor> ErrorTableSingles { get; set; }
        public DbSet<ProcessedTableCompressor> ProcessedTableSingles { get; set; }
        public DbSet<PredictedTableCompressor> PredictedTableSingles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-CGG65T8;Initial Catalog=DPM;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchTable>().ToTable("Batch");
            modelBuilder.Entity<BatchTable>().HasKey(c => c.Id);
            modelBuilder.Entity<BatchTable>()
                    .Property(p => p.Description)
                    .HasColumnType("varchar(200)");

            modelBuilder.Entity<StagingTableCompressor>().ToTable("Compressor_Staging");
            modelBuilder.Entity<StagingTableCompressor>().HasKey(c => c.Id);

            modelBuilder.Entity<CleanTableCompressor>().ToTable("Compressor_Cleaning");
            modelBuilder.Entity<CleanTableCompressor>().HasKey(c => c.Id);

            modelBuilder.Entity<ErrorTableCompressor>().ToTable("Compressor_Error");
            modelBuilder.Entity<ErrorTableCompressor>().HasKey(c => c.Id);

            modelBuilder.Entity<ProcessedTableCompressor>().ToTable("Compressor_Processed");
            modelBuilder.Entity<ProcessedTableCompressor>().HasKey(c => c.Id);

            modelBuilder.Entity<PredictedTableCompressor>().ToTable("Compressor_Predicted");
            modelBuilder.Entity<PredictedTableCompressor>().HasKey(c => c.Id);


        }
    }

}




