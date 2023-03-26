using Microsoft.EntityFrameworkCore;
using HallOfFame.Models;


namespace HallOfFame.Models;

public class HellOfFameContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Skills> Skills { get; set; }

    public HellOfFameContext()
    {
        
    }
    
    public HellOfFameContext(DbContextOptions<HellOfFameContext> options)
        : base(options)
    {
    }
}