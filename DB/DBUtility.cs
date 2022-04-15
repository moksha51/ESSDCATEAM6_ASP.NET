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

            SeedProduct();
        }



        public void SeedProduct()
        {
            dbContext.Add(new Products
            {
                ProductName = "Outbyte PC Repair",
                UnitPrice = 50.80,
                ProductDescription = "Outbyte PC Repair is a software that helps you to clean your registry with no hassle. This application quickly secures your PC and stops the computer from freezing and crashing. It safely repairs your computer to an optimized state.This one of the best PC cleaning software allows you to repair damages made by the virus.",
                IconURL = "image1.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 2,
                ProductName = "NordVPN",
                UnitPrice = 198.00,
                ProductDescription = "NordVPN is a software which does not track, collect, or share data. It is available on Android, Windows, Apple, macOS, and Linux. You can enjoy fast connection without buffering. This software does not store session information, used bandwidth, IP addresses, traffic data, and session details.Offers 24/7 product support.",
                IconURL = "image2.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 3,
                ProductName = "Adobe Photoshop Express Editor",
                UnitPrice = 315.61,
                ProductDescription = "Adobe Photoshop Express Editor is an application for photo retouching and image editing. It can be used by designers, graphic artists, photographers, web developers, and creative professionals. This tool offers to create, enhance, edit artworks, images, and illustrations. Adobe Photoshop Express Editor has a motion blur gallery that includes two effects, spin blur and path blur. Provides a tool for path selection.",
                IconURL = "image3.png"
             });

            dbContext.Add(new Products
            {
                ProductId = 4,
                ProductName = "Icedrive",
                UnitPrice = 140.20,
                ProductDescription = "Icedrive is a next-generation cloud service that helps you to access, manage, and update your cloud storage effortlessly. It provides a space to share, showcase, and collaborate with your files. It allows you to store files up to 10 GB for free. Icedrive provides clean and easy to use interface to manage your files.",
                IconURL = "image4.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 5,
                ProductName = "pCloud",
                UnitPrice = 100.00,
                ProductDescription = "pCloud is secure and simple to use cloud storage for your documents. This tool allows you to share and collaborate files with your employee, friends, and relatives. Allows to backup your valuable memories to pCloud by just clicking a click of a button. It helps you to filter your files according to their file format.",
                IconURL = "image5.png"
             });

            dbContext.Add(new Products
            {
                ProductId = 6,
                ProductName = "Dropbox",
                UnitPrice = 19.90,
                ProductDescription = "Dropbox is a file hosting service providing personal cloud, file synchronization, cloud storage, and client software. This tool is designed to collaborate on your projects, whether you are working alone or in a team. It offers you to safely sync data across all devices and store data up to 50 GB.",
                IconURL = "image6.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 7,
                ProductName = "planner5d",
                UnitPrice = 201.75,
                ProductDescription = "Planner 5D is another important 3D alternative tool that enables you to create realistic interior and exterior designs in 2D/ 3D modes. It allows you to use the Snapshots feature to capture your design as a realistic image. Both 2D and 3D views as you design from various angles. Apply custom colors, patterns, and materials to furniture",
                IconURL = "image7.png"
             });

            dbContext.Add(new Products
            {
                ProductId = 8,
                ProductName = "Recuva",
                UnitPrice = 135.65,
                ProductDescription = "Recuva is a data recovery software for Windows 10. It helps you recover files on your hard drive, memory cards, floppy disks, iPods, MP3 players, etc. Recuva can also retrieve data from newly formatted or damaged drives. It allows you to retrieve essential data from a deleted or damaged disk. You can also store and restore unsaved word documents.",
                IconURL = "image8.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 9,
                ProductName = "EaseUS Todo Backup ",
                UnitPrice = 88.80,
                ProductDescription = "EaseUS Todo Backup software product allows you to clone a smaller hard disk drive to a larger hard disk drive. It also allows you to clone HDD to SSD for increasing performance. This software product helps you to recover deleted data or backup your system.",
                IconURL = "image9.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 10,
                ProductName = "Autodesk SketchBook",
                UnitPrice = 230.90,
                ProductDescription = "Autodesk is free to use a drawing tool, creating a sketching tool, which helps you helps you to create quick conceptual sketches to fully finished artwork. It is one of the best software for PC that offers unique features like gallery file recovery and Dex support. It allows you to access all the drawing and sketching tools on desktop and mobile platforms.",
                IconURL = "image10.png"
             });

            dbContext.Add(new Products
            {
                ProductId = 11,
                ProductName = "Adobe Photoshop Elements",
                UnitPrice = 315.61,
                ProductDescription = "Adobe Photoshop Elements 2019 is a photo editing software. This graphic drawing tool offers easy ways to get started, effortless organization, step-by-step guidance for editing. The tool allows you to auto-generate photos, videos, and slideshows.",
                IconURL = "image11.png"
             });

            dbContext.Add(new Products
            {
                ProductId = 12,
                ProductName = "Filmora",
                UnitPrice = 116.95,
                ProductDescription = "Filmora is a powerful video editor for Windows users. It has an intuitive user interface and lots of video effects. This tool has advanced editing modes and features. This tool provides keyframing, motion tracking, silence detection and speed ramping which help you be more productive.",
                IconURL = "image12.png"
            });

            dbContext.Add(new Products
            {
                ProductId = 13,
                ProductName = "Spotify",
                UnitPrice = 99.90,
                ProductDescription = "Spotify is a music streaming app. This application helps you to find the music or podcast. It contains numerous episodes and tracks. You can use this program and browse artists, albums, celebrities, and more.",
                IconURL = "image13.png"
            });





            dbContext.SaveChanges();
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
