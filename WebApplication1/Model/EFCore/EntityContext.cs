using Microsoft.EntityFrameworkCore;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.EFCore
{
    public class EntityContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("Data Source=localhost;user id=root;password=123456;Initial Catalog=tassdar");
            optionsBuilder.UseSqlServer("Server =.; Database = dbCore; User ID = sa; Password = A123456~; ");
        }
        public DbSet<t_user> t_users { get; set; }

        public DbSet<t_system> t_systems { get; set; }

    }
}
