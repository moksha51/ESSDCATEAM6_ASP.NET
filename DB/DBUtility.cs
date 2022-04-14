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
            //SeedProduct();
        }

       

        //public void SeedProduct()
        //{
        //        dbContext.Add(new Products
        //        {
        //            ProductId = 1,
        //            ProductName = "OutByte",
        //            UnitPrice = 00,
        //            ProductDescription = "OutByte",
        //            IconURL = "image1.png"
        //        });
        //        dbContext.Add(new Products
        //        {
        //            ProductId = 2,
        //            ProductName = "NordVPN",
        //            UnitPrice = 00,
        //            ProductDescription = "NordVPN",
        //            IconURL = "image2.png"
        //        });
        //        dbContext.Add(new Products
        //        {
        //            ProductId = 3,
        //            ProductName = "Adobe Photoshp Express Editor",
        //            UnitPrice = 00,
        //            ProductDescription = "Adobe Photoshp Express Editor",
        //            IconURL = "image3.png"
        //        });

        //        dbContext.Add(new Products
        //        {
        //            ProductId = 4,
        //            ProductName = "Icedrive​",
        //            UnitPrice = 00,
        //            ProductDescription = "Icedrive",
        //            IconURL = "image4.png"
        //        });
        //        dbContext.Add(new Products
        //        {
        //            ProductId = 5,
        //            ProductName = "pCloud",
        //            UnitPrice = 00,
        //            ProductDescription = "pCloud",
        //            IconURL = "image5.png"
        //        });

        //    dbContext.SaveChanges();
        //    }

            
            
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
