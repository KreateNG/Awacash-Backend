using System;
using Awacash.Shared;
using MediatR;

namespace Awacash.Application.Authentication.Handler.Commands.AdminForgotPassword
{
	public record AdminForgotPasswordCommand(string Email): IRequest<ResponseModel<string>>;
}

