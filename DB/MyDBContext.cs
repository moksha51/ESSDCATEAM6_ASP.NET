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
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

    }
}
