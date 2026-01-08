using Microsoft.EntityFrameworkCore;
using CampusEats.Persistence.Entities;

namespace CampusEats.Persistence.Context;

public class CampusEatsDbContext(DbContextOptions<CampusEatsDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    
    public DbSet<MenuItem> MenuItems { get; set; } = null!;
    public DbSet<MenuLabel> MenuLabels { get; set; } = null!;
    public DbSet<MenuItemLabel> MenuItemLabels { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<MenuItemIngredient> MenuItemIngredients { get; set; } = null!;
    
    public DbSet<Favorite> Favorites { get; set; } = null!;
    
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<PaymentEvent> PaymentEvents { get; set; } = null!;
    
    public DbSet<Recommendation> Recommendations { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.StudentId);


        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.StudentId, f.MenuItemId });

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Student)
            .WithMany(s => s.Favorites)
            .HasForeignKey(f => f.StudentId);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.MenuItem)
            .WithMany()
            .HasForeignKey(f => f.MenuItemId);
        
        modelBuilder.Entity<MenuItemLabel>()
            .HasKey(mil => new { mil.MenuItemId, mil.LabelId });

        modelBuilder.Entity<MenuItemLabel>()
            .HasOne(mil => mil.MenuItem)
            .WithMany(mi => mi.MenuItemLabels)
            .HasForeignKey(mil => mil.MenuItemId);

        modelBuilder.Entity<MenuItemLabel>()
            .HasOne(mil => mil.Label)
            .WithMany(l => l.MenuItemLabels)
            .HasForeignKey(mil => mil.LabelId);
        
        modelBuilder.Entity<MenuItemIngredient>()
            .HasKey(mii => new { mii.MenuItemId, mii.IngredientId });

        modelBuilder.Entity<MenuItemIngredient>()
            .HasOne(mii => mii.MenuItem)
            .WithMany(mi => mi.MenuItemIngredients)
            .HasForeignKey(mii => mii.MenuItemId);

        modelBuilder.Entity<MenuItemIngredient>()
            .HasOne(mii => mii.Ingredient)
            .WithMany(i => i.MenuItemIngredients)
            .HasForeignKey(mii => mii.IngredientId);

        
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany()
            .HasForeignKey(oi => oi.MenuItemId);
        
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.MenuItem)
            .WithMany()
            .HasForeignKey(ci => ci.MenuItemId);
        
        modelBuilder.Entity<PaymentEvent>()
            .HasOne(pe => pe.Payment)
            .WithMany(p => p.PaymentEvents)
            .HasForeignKey(pe => pe.PaymentId);
        
        modelBuilder.Entity<Recommendation>()
            .HasKey(r => new { r.BaseItemId, r.RecommendedItemId });

        modelBuilder.Entity<Recommendation>()
            .HasOne(r => r.BaseItem)
            .WithMany()
            .HasForeignKey(r => r.BaseItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Recommendation>()
            .HasOne(r => r.RecommendedItem)
            .WithMany()
            .HasForeignKey(r => r.RecommendedItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<MenuItem>()
            .Property(mi => mi.IsAvailable)
            .HasDefaultValue(true);
    }
}
