using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalKinetic_CoinJar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace GlobalKinetic_CoinJar.Data
{
    public class SysContext : IdentityDbContext
    {
        public SysContext(DbContextOptions<SysContext> options) : base(options) { }

        public DbSet<Coins> Coins { get; set; }

        public DbSet<CoinJars> CoinJars { get; set; } 
    }
}
