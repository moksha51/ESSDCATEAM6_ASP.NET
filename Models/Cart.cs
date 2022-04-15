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

        }
        
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public virtual Products Product { get; set; } 
        public virtual User UserId { get; set; }
        [Required]
        public virtual Session SessionId { get; set; }
    }
}
