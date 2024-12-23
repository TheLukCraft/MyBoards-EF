using Microsoft.EntityFrameworkCore;

namespace MyBoards.Entities;

public class MyBoardsContext(DbContextOptions<MyBoardsContext> options) : DbContext(options)
{
    public required DbSet<WorkItem> WorkItems { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Tag> Tags { get; set; } 
    public required DbSet<Comment> Comments { get; set; }
    public required DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkItem>(eb =>
        {
            eb.Property(x => x.State).IsRequired();
            eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");
            eb.Property(wi => wi.Efford).HasColumnType("decimal(5,2)");
            eb.Property(wi => wi.EndDate).HasPrecision(3);
            eb.Property(wi => wi.Activity).HasMaxLength(200);
            eb.Property(wi => wi.RemaningWork).HasPrecision(14, 2);
            eb.Property(wi => wi.Priority).HasDefaultValue(1);
            eb.HasMany(w => w.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

            eb.HasOne(w => w.Author)
                .WithMany(u => u.WorkItems)
                .HasForeignKey(w => w.AuthorId)
        });

        modelBuilder.Entity<Comment>(eb =>
        {
            eb.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
            eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
        });

        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);
    }

}
