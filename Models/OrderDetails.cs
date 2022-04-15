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
            SerialCode = Id.ToString();
        }
        public Guid Id { get; set; }
        [Required]
        public virtual int ProductId { get; set; } 
        [Required]
        public string SerialCode { get; set; } 
        public virtual Guid OrdersId { get; set; }

    }
}
