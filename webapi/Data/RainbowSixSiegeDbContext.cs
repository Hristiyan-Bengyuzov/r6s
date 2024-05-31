using Microsoft.EntityFrameworkCore;
using webapi.Data.Models;

namespace webapi.Data
{
    public class RainbowSixSiegeDbContext : DbContext
    {
        public RainbowSixSiegeDbContext(DbContextOptions<RainbowSixSiegeDbContext> options) : base(options) { }

        public DbSet<Operator> Operators { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Gadget> Gadgets { get; set; }
        public DbSet<OperatorWeapon> OperatorWeapons { get; set; }
        public DbSet<OperatorGadget> OperatorGadgets { get; set; }
    }
}
