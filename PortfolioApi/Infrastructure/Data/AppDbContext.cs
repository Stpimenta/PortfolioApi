using Microsoft.EntityFrameworkCore;
using PortfolioApi.Domain.Entities;


namespace PortfolioApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<Icons> Icons { get; set; }
    public DbSet<UserRoleProgress> UserRoleProgress { get; set; }
    public DbSet<UserTechProgress> UserTechProgress { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRoleProgress>()
            .HasKey(urp => new { urp.UserId, urp.RoleId });

        modelBuilder.Entity<UserRoleProgress>()
            .HasOne(urp => urp.User)
            .WithMany(u => u.UserRoles) 
            .HasForeignKey(urp => urp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRoleProgress>()
            .HasOne(urp => urp.Role)
            .WithMany(r => r.UserRoles) 
            .HasForeignKey(urp => urp.RoleId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<UserTechProgress>()
            .HasKey(utp => new { utp.UserId, utp.TechId });

        modelBuilder.Entity<UserTechProgress>()
            .HasOne(utp => utp.User)
            .WithMany(u => u.UserTechnologies)
            .HasForeignKey(utp => utp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserTechProgress>()
            .HasOne(utp => utp.Tech)
            .WithMany(t => t.UserTechnologies)
            .HasForeignKey(utp => utp.TechId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Role>()
            .HasOne(r => r.Icon)
            .WithMany() 
            .HasForeignKey(r => r.IconId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasMany(p => p.Technologies)
                .WithMany(t => t.Projects);

            entity.Property(p => p.Images)
                .HasColumnType("text[]");

        });

        modelBuilder.Entity<Technology>()
            .HasOne(t => t.Icon)
            .WithMany(i => i.Technologies)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UserRoleProgress>()
            .HasMany(urp => urp.Projects)
            .WithMany(p => p.UserRoleProgresses);
           

        base.OnModelCreating(modelBuilder);
    }
}