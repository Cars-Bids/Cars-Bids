using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsAndBids.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarsAndBids.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        //public ApplicationDbContext() { }
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options)
        //{
        //}

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=ep-round-poetry-a25e8nz8-pooler.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_KxuaC0oX6tci");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування індексів для оптимізації пошуку
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Auction>()
                .HasIndex(a => new { a.Make, a.Model, a.Year });

            modelBuilder.Entity<Bid>()
                .HasIndex(b => b.AuctionId);

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.AuctionId);

            // Налаштування зв’язків
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Seller)
                .WithMany(u => u.Auctions)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Bidder)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Auction)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.ProfilePictureUrl)
                .IsRequired(false);

            modelBuilder.Entity<Auction>()
                .Property(a => a.Make)
                .IsRequired();

            modelBuilder.Entity<Auction>()
                .Property(a => a.Model)
                .IsRequired();

            modelBuilder.Entity<Auction>()
                .Property(a => a.Description)
                .IsRequired(false);

            modelBuilder.Entity<Auction>()
                .Property(a => a.Status)
                .IsRequired()
                .HasDefaultValue("Draft");

            modelBuilder.Entity<Auction>()
                .Property(a => a.VIN)
                .IsRequired(false);

            modelBuilder.Entity<Auction>()
                .Property(a => a.Location)
                .IsRequired(false);

            modelBuilder.Entity<Comment>()
                .Property(c => c.Content)
                .IsRequired();
        }
    }
}
