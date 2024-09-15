using Microsoft.EntityFrameworkCore;

using WebApi.Models;

namespace WebApi.Data;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}