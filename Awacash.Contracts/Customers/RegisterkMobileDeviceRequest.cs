using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Contracts.Customers
{
    public record RegisterkMobileDeviceRequest(string Phone, string DeviceId, string Otp);
}
