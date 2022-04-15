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
            Cart = new List<Cart>();
            OrderDetails = new List<OrderDetails>();
        }

        public Guid Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string IconURL { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double UnitPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        public virtual ICollection<Cart> Cart {get;set;}
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }



    }


}
