using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Promotions
{
    public record AddPromotionRequest(string Title, string Description, bool HasImage, string? Link, string? Base64File);
}
