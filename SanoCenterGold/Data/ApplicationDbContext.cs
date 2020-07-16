using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SanoCenterGold.Models;

namespace SanoCenterGold.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SanoCenterGold.Models.Ejercicio> Ejercicio { get; set; }
        public DbSet<SanoCenterGold.Models.Reto> Reto { get; set; }
    }
}
