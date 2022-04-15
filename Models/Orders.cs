using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CATeam6.Models
{
    public class Orders
    {
        public Orders()
        {
            Id = new Guid();
            OrderDetails = new List<OrderDetails>();
            Products = new List<Products>();
            //ActCodes = new List<ActCode>();

        }
        public Guid Id { get; set; }

        [Required]
        public DateTime OrderDateTime {get;set;}

        [Required]
        public virtual ICollection<Products> Products { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        [Required]
        public virtual User UserId { get; set; }

    }
}
