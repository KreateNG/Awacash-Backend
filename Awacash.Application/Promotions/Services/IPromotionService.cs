using Awacash.Application.Promotions.DTOs;
using Awacash.Shared;
using Awacash.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Promotions.Services
{
    public interface IPromotionService
    {
        Task<ResponseModel<bool>> CreatePromotion(string title, string description, bool hasImage, string? Link,
        string? base64File = null);
        Task<ResponseModel<bool>> UpdatePromotion(string title, string description, bool hasImage);
        Task<ResponseModel<bool>> UpdatePromotionImage(string id, string? base64File = null);
        Task<ResponseModel<bool>> UpdatePromotionStatus(string id);
        Task<ResponseModel<PromotionDTO>> GetPromotionById(string id);
        Task<ResponseModel<List<PromotionDTO>>> GetAllPromotion();
        Task<ResponseModel<PagedResult<PromotionDTO>>> GetPagedPromotion();

    }
}
