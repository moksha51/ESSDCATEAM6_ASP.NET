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
            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public Guid Id { get; set; }
        public long Timestamp { get; set; }
        public virtual string Username { get; set; }
    }
}
