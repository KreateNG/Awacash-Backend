using System;
namespace Awacash.Application.Common.Model
{
    public class BvnVerificationModel
    {
        public DetealModel? Bnv { get; set; }
        public DetealModel? Customer { get; set; }
    }

    public class DetealModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}

