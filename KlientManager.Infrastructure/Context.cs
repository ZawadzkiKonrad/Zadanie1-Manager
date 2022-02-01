using KlientManager.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlientManager.Infrastructure
{
    public class Context:DbContext  //lub IdentityDbContext

    {
        public DbSet<Klienci> Klients { get; set; }
        public Context(DbContextOptions options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
