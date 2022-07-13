using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SoleCode.Api.Entities
{
    public partial class EntityDbContext : DbContext
    {

        public EntityDbContext(DbContextOptions<EntityDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<TodoList> TodoLists { get; set; }
        public virtual DbSet<TodoListHistory> TodoListHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString") ?? throw new InvalidOperationException());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasKey(c => new { c.UID });
                entity.Property(e => e.UID).HasColumnName("UID");

                entity.Property(e => e.Username).HasColumnName("Username").IsRequired();
                entity.Property(e => e.Password).HasColumnName("Password").IsRequired();
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CreatedDate")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UpdatedDate")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<TodoList>(entity =>
            {
                entity.ToTable("TodoList");

                entity.HasKey(c => new { c.UID });
                entity.Property(e => e.UID).HasColumnName("UID");

                entity.Property(e => e.Name).HasColumnName("Name").IsRequired();
                entity.Property(e => e.Status).HasColumnName("Status").IsRequired();
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CreatedDate")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UpdatedDate")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<TodoListHistory>(entity =>
            {
                entity.ToTable("TodoListHistory");

                entity.HasKey(c => new { c.UID });
                entity.Property(e => e.UID).HasColumnName("UID");

                entity.Property(e => e.RowUID).HasColumnName("RowUID").IsRequired();
                entity.Property(e => e.NewData).HasColumnName("NewData");
                entity.Property(e => e.OldData).HasColumnName("OldData");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CreatedDate")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public class TestModuleContextFactory : IDesignTimeDbContextFactory<EntityDbContext>
    {
        public EntityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EntityDbContext>();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString") ?? throw new InvalidOperationException());

            return new EntityDbContext(optionsBuilder.Options);
        }
    }
}
