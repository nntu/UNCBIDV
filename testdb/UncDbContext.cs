using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace UyNhiemChiBIDV
{
    internal class UncDbContext : DbContext
    {
        public UncDbContext()
            : base("UncDbContext")
        {

        }
        public DbSet<inUNC> Uncs { get; set; }

    }
}
