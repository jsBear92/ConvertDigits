using Microsoft.EntityFrameworkCore;

namespace ConvertDigits.Model;
public class NumberContext : DbContext
{
    public NumberContext(DbContextOptions<NumberContext> options) : base(options)
    {
    }

    public DbSet<Numbers> Numbers { get; set; } = null!;
}