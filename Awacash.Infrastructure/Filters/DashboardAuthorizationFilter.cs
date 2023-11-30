using System;
using Hangfire.Dashboard;
using Hangfire.Annotations;

namespace Awacash.Infrastructure.Filters;

public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
}

