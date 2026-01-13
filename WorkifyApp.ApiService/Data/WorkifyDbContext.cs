using Microsoft.EntityFrameworkCore;
using WorkifyApp.ApiService.Domain;

namespace WorkifyApp.ApiService.Data;

public class WorkifyDbContext(DbContextOptions<WorkifyDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Client
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // Configure Project
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Invoice
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Client)
            .WithMany()
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure InvoiceLineItem
        modelBuilder.Entity<InvoiceLineItem>()
            .HasOne(i => i.Invoice)
            .WithMany(inv => inv.Items)
            .HasForeignKey(i => i.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
