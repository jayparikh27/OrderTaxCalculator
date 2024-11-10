using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaxCalculator.Domain.Entities
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Not ALL the models are created. This is just an example.
            // Configuring relationships
            modelBuilder.Entity<TaxRate>()
                .HasOne(t => t.State)
                .WithMany(s => s.TaxRates)
                .HasForeignKey(t => t.StateID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Client>()
                .HasOne(c => c.State)
                .WithMany(s => s.Clients)
                .HasForeignKey(c => c.StateID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configuring Product and ProductCategory relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configuring Coupon and Product relationship
            modelBuilder.Entity<Coupon>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Coupons)
                .HasForeignKey(c => c.ProductID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
        }
    }
    }

