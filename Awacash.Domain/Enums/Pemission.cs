using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace Awacash.Domain.Enums
{
    public enum Pemission
    {
        [Display(GroupName = "Audit Logs", Name = "View", Description = "Can view audit logs", ShortName = "Audit_Log_View")]
        Audit_Log_View = 0x10,



        [Display(GroupName = "Customer Management", Name = "View", Description = "Can view customers")]
        Customer_View = 0x30,
        [Display(GroupName = "Customer Management", Name = "Update", Description = "Can update customer record")]
        Customer_Update = 0x31,

    }
}

