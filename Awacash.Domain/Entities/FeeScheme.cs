//using Awacash.Domain.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Awacash.Domain.Entities
//{
//    public class FeeScheme:BaseEntity
//    {
//        public FeeScheme()
//        {
//            this.FeeSchemeConfigurations = new HashSet<FeeSchemeConfiguration>();
//        }
//        public string SchemeName { get; set; }

//        public string Description { get; set; }
//        public bool IsDefault { get; set; }

//        public Status Status { get; set; }

//        public virtual ICollection<FeeSchemeConfiguration> FeeSchemeConfigurations { get; set; }
//    }
//}
