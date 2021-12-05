using Microsoft.EntityFrameworkCore;

namespace TodoManagerApp.DAL.Data
{
    public class DbTaskManagerContext : DbContext
    {
        public DbTaskManagerContext()
        {
        }

        public DbTaskManagerContext (DbContextOptions<DbTaskManagerContext> options) : base(options)
        {
        }
        
        public DbSet<Column> Columns { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Column>(entity =>
            {
                entity.ToTable("Column");
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.HasIndex(e => e.Priority).IsUnique();
            }
            );
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("Todo");
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.Priority);
                entity.HasIndex(e => new { e.Priority, e.ColumnID }).IsUnique();
                entity.HasOne(d => d.Column)
                .WithMany(t => t.Todos)
                .HasForeignKey(d => d.ColumnID)
                .OnDelete(DeleteBehavior.Cascade);
            }
            );
        }
    }
}
