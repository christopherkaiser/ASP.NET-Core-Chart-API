using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Entities
{
    public class ChartContext : DbContext
    {
        public ChartContext(DbContextOptions<ChartContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Chart> Charts { get; set; }
        public DbSet<ValuePair> ValuePairs { get; set; }
    }
}
