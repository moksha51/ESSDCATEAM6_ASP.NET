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
        public virtual ICollection<Products> ProductId { get; set; } //To consider changing toa regular string; regular referencing can be made, no need to link tables

        [Required]
        public string SerialCode { get; set; } //generated at checkout?

        public virtual Guid OrdersId { get; set; }

    }
}
