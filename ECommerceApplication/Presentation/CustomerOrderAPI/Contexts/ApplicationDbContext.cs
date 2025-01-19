using CustomerOrderAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace CustomerOrderAPI.Contexts;

public class ApplicationDbContext
    : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderInboxMessage> OrderInboxMessages { get; set; }
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}
