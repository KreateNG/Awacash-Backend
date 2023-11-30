using System;
using Awacash.Application.Authentication.Common;
using Awacash.Application.Authentication.Services;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.RefreshToken;
public class RefreshTokenCommand : IRequest<ResponseModel<AuthenticationResult>>
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().NotNull().WithMessage("Current token is required");
        RuleFor(x => x.RefreshToken).NotEmpty().NotNull().WithMessage("Current refersh is required");
    }
}
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseModel<AuthenticationResult>>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ResponseModel<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
    }
}



