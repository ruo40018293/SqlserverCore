using Common.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model.EntityCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace DAL
{
    public class EFDbcontext:DbContext
    {
        public EFDbcontext(DbContextOptions<EFDbcontext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql(ConfigHelper.GetValue<string>("ConnectionsStrings:Development"));
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            string path = Thread.GetDomain().BaseDirectory;
            Assembly[] assemblies = new DirectoryInfo(path).GetFiles("*.dll", SearchOption.AllDirectories)
                .Select(x => Assembly.LoadFrom(x.FullName)).ToArray();
            Type baseType = typeof(EntityBase);
            foreach (var assembly in assemblies)
            {
                IEnumerable<Type> entities = assembly.GetTypes().Where(x => x.IsSubclassOf(baseType));
                foreach (var entity in entities)
                {
                    modelBuilder.Entity(entity);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

    }
}
