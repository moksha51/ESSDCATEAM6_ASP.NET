using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CATeam6.Models;

namespace CATeam6.Models
{
    public class Products
    {
        public Products()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
        [Required]
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public float UnitPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }

        public virtual ICollection<Cart> CartId {get;set;}

        public virtual ICollection<Orders> Orders { get; set; }

        public string IconURL { get; set; }

    }


}
