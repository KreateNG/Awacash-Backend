using Awacash.Application.Savings.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Queries.GetSavingById
{
    public record GetSavingByIdQuery(string Id) : IRequest<ResponseModel<SavingDTO>>;
}
