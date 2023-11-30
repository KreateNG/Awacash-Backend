using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Token : BaseEntity
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Code { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? ExpiryDate { get; set; }
        public string ResourceId { get; set; } = default!;
        public string ResourceName { get; set; } = default!;

        public void UpdateLastModifiedDate()
        {
            ModifiedDate = DateTime.UtcNow;
        }

        public void MakeDeleted(string deletedBy)
        {
            IsDeleted = true;
            ModifiedDate = DateTime.UtcNow;

        }
    }
}
