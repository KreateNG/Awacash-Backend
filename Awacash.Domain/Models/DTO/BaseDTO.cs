using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.DTO
{
    public record BaseDTO
    {
        public string? Id { get; private set; }
        public string? CreatedBy { get; set; }

        public string? CreatedByIp { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public string? ModifiedByIp { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
