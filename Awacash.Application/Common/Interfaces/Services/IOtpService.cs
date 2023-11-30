using System;
using Awacash.Shared;
using AwaCash.Application.Common.Model;

namespace AwaCash.Application.Common.Interfaces.Services
{
    public interface IOtpService
    {
        Task<ResponseModel<OtpResponseDTO>> CreateAsync(string ResourceId, string ResourceName);
        Task<ResponseModel<OtpResponseDTO>> Reset(string id);
        Task<ResponseModel<OtpResponseDTO>> Use(string id);
        Task<ResponseModel<OtpResponseDTO>> GetSingleAsync(string id);
        Task<ResponseModel<string>> DeleteAsync(string id);
    }
}

