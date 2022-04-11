using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CATeam6.Models
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }

        [Required]
        public virtual ICollection<Products> ProductId { get; set; }

        [Required]
        public string SerialCode { get; set; }
    }
}
