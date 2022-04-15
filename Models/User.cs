using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CATeam6.Models
{
    public class User
    {
        public User()
        {
            Id = new Guid();
            Orders = new List<Orders>();
            Cart = new List<Cart>();

        }

        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] PassHash { get; set; }
        [Required]
        //public virtual Session SessionId { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }



    }
}
