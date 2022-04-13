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
        

    }
}
