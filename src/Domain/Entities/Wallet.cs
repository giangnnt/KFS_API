using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace KFS.src.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public int Point { get; set; }
        public Guid UserId { get; set; }
        public User Owner { get; set; } = null!;
    }
}