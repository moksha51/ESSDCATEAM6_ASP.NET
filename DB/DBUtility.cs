using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using CATeam6.Models;

namespace CATeam6.DB
{
    public class DBUtility
    {
        private MyDBContext dbContext;
        public DBUtility (MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            SeedUsers();
            // TODO: add SeedProducts() and respective products
        }

        public void SeedUsers()
        {
            HashAlgorithm sha = SHA256.Create();
            string[] usernames = { "john", "lisa" };
            string password = "secret";
            foreach (string username in usernames)
            {
                string combo = username + password;
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combo));
                dbContext.Add(new User
                {
                    Username = username,
                    PassHash = hash
                });
                dbContext.SaveChanges();
            }
        }
    }
}
