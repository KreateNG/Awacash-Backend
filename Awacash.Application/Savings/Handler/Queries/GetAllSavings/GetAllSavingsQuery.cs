using Awacash.Application.Savings.DTOs;
using Awacash.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Application.Savings.Handler.Queries.GetAllSavings
{
    public record GetAllSavingsQuery():IRequest<ResponseModel<List<SavingDTO>>>;
}
