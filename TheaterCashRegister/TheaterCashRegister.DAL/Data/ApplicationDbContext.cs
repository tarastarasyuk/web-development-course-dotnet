using Microsoft.EntityFrameworkCore;
using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // uniqueness
        modelBuilder.Entity<Performance>()
            .HasIndex(p => p.Title)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => new { t.PerformanceId, t.SeatNumber })
            .IsUnique();

        // cascade delete
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Ticket)
            .WithOne(i => i.Booking)
            .HasForeignKey<Booking>(b => b.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Performance)
            .WithMany(g => g.Tickets)
            .HasForeignKey(t => t.PerformanceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Performance> Performance { get; set; }
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<Booking> Booking { get; set; }
}