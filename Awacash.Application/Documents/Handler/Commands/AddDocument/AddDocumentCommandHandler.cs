using System;
using Awacash.Application.Documents.Services;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Documents.Handler.Commands.AddDocument
{
    public class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, ResponseModel<bool>>
    {
        private readonly IDocumentService _documentService;
        public AddDocumentCommandHandler(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task<ResponseModel<bool>> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            return await _documentService.CreateDocuent(request.IdBase64, request.UtilityBase64, request.IDNumber, request.FileType);
        }
    }
}

