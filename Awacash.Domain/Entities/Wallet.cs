using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public string? CustomerId { get; set; }
        public decimal? Balance { get; set; }
        public string? Status { get; set; }
        public virtual Customer? Customer { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? CheckSum { get; set; }
    }
}
