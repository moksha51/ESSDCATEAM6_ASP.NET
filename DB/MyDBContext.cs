using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CATeam6.Models;
using Microsoft.EntityFrameworkCore;

namespace CATeam6.DB
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        // maps to a Sessions table in our database
        public DbSet<Session> Sessions { get; set; }
        //TODO: add the rest of the tables here 
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Products> Products { get; set; }

    }
}
