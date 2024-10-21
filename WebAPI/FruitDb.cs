using Microsoft.EntityFrameworkCore;

public class FruitDb : DbContext
{
    public FruitDb(DbContextOptions options)
        : base(options) { }

    public DbSet<Fruit> Fruits { get; set; }
}
