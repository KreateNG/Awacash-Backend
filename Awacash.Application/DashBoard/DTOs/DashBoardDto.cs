using System;
using Awacash.Application.Transactions.DTOs;

namespace Awacash.Application.DashBoard.DTOs
{
    public class DashBoardDto
    {
        public int TotalCustomers { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalTransactionVolume { get; set; }
        public int TotalCardRequest { get; set; }
        public List<TransactionDTO> Transactions { get; set; } = new List<TransactionDTO>();
    }
}

