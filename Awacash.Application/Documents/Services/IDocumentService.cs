using System;
using Awacash.Domain.Entities;
using Awacash.Shared;

namespace Awacash.Application.Documents.Services
{
    public interface IDocumentService
    {
        Task<ResponseModel<bool>> CreateDocuent(string? idBase64, string? utilityBase64, string? IDNumber, FileType? FileType);
    }
}

