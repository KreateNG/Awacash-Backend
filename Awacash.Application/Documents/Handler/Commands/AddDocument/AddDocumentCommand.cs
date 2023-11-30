using System;
using Awacash.Domain.Entities;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Documents.Handler.Commands.AddDocument
{
    public record AddDocumentCommand(string? IdBase64, string? UtilityBase64, string? IDNumber, FileType? FileType) : IRequest<ResponseModel<bool>>;

}

