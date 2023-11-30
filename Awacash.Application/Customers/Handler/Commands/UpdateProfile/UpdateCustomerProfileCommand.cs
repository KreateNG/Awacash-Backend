using System;
using Awacash.Application.Common.Model;
using Awacash.Application.Customers.Services;
using Awacash.Shared;
using FluentValidation;
using MediatR;

namespace Awacash.Application.Customers.Handler.Commands.UpdateProfile;

public record UpdateCustomerProfileCommand(string? LastName, string? FirstName, string? MiddleName) : IRequest<ResponseModel<CustomerDTO>>;


public class UpdateCustomerProfileCommandValidator : AbstractValidator<UpdateCustomerProfileCommand>
{
    public UpdateCustomerProfileCommandValidator()
    {
        RuleFor(x => x.LastName).NotEmpty().NotNull().WithMessage("Last name is required");
        RuleFor(x => x.FirstName).NotEmpty().NotNull().WithMessage("First name is required");
    }
}
public class UpdateCustomerProfileCommandHandler : IRequestHandler<UpdateCustomerProfileCommand, ResponseModel<CustomerDTO>>
{
    private readonly ICustomerService _customerService;
    public UpdateCustomerProfileCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<ResponseModel<CustomerDTO>> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        return await _customerService.UpdateProfile(request.LastName, request.FirstName, request.MiddleName);
    }
}

