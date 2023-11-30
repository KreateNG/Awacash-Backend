using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Models.Customer
{
    public class BvnCustomerInfo
    {
        public string? marital_status { get; set; }
        public string? gender { get; set; }
        public string? surname { get; set; }
        public string? middle_name { get; set; }
        public string? first_name { get; set; }
        public string? nationality { get; set; }
        public string? state_of_origin { get; set; }
        public string? lga_of_origin { get; set; }
        public string? state_of_residence { get; set; }
        public string? lga_of_residence { get; set; }
        public string? email { get; set; }
        public string? state_of_capture { get; set; }
        public string? lga_of_capture { get; set; }
        public string? nin { get; set; }
        public string? Phone_number1 { get; set; }
        public string? phone_number2 { get; set; }
        public string? enrollment_date { get; set; }
        public string? enroll_bank_code { get; set; }
        public string? DateOfBirth { get; set; }
        public string? name_on_card { get; set; }
        public string? title { get; set; }
        public string? branch_name { get; set; }
        public int AccountDetailId { get; set; }
        public int ImageDetailsId { get; set; }
    }
}
