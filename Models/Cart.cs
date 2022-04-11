using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CATeam6.Models
{
    public class Cart
    {
        public Cart()
        {
            CartId = new Guid();
            Products = new List<Products>();
        }
        
        public Guid CartId { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<Products> Products { get; set; }

        public virtual User UserId { get; set; }

        public virtual Session SessionId { get; set; }


    }
}
