using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Entities
{
    public class Promotion : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? ImageUrl { get; set; }
        public bool HasImage { get; set; }
        public Status Status { get; set; }

    }
}
