using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CATeam6.Models
{
    public class Session
    {
        public Session()
        {
            
            SessionId = new Guid();
        }
        public Guid SessionId { get; set; }
        [Required]
        public DateTime SessionDateTime { get; set; }
        [Required]
        public virtual User UserId { get; set; }
    }
}
