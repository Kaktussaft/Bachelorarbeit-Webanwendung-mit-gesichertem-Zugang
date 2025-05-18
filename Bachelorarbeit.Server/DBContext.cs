using Bachelorarbeit.Server.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bachelorarbeit.Server;

public class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configuration;
    
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.Surname)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.PasswordSalt)
                .IsRequired();

            entity.Property(e => e.RefreshToken);

            entity.Property(e => e.IsAdmin)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

    }

    
}