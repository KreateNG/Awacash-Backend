using Awacash.Application.Common.Model;
using Awacash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.DTOs
{
    public class PromotionDTO : BaseDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool HasImage { get; set; }
        public Status Status { get; set; }
        public string? Link { get; set; }
    }
}
