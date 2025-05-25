using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiCarsBids.Data.Entities;
using WebApiCarsBids.Data.Entities.Identity;

namespace WebApiCarsBids.Data;

public class CarsBidsDbContext : IdentityDbContext<UserEntity, RoleEntity, long>
{
    public CarsBidsDbContext(DbContextOptions<CarsBidsDbContext> opt) : base(opt) { }
    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserRoleEntity>(ur =>
        {
            ur.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            ur.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
        });
    }
}
