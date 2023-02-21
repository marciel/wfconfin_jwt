using System;
using Microsoft.EntityFrameworkCore;
using WFConFin.Models;

namespace WFConFin.Data
{
	public class WFConFinContext: DbContext
	{

        public WFConFinContext
            (DbContextOptions<WFConFinContext> options)
            : base(options)
        {

        }

        public DbSet<Estado> Estado { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}

