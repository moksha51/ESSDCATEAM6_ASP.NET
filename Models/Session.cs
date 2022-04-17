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
            Id = new Guid();
            Timestamp = DateTimeOffset.Now;
        }

        public Guid Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public virtual User User { get; set; }
    }
}
