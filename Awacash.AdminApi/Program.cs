using Audit.Core;
using Awacash.Application;
using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Role.Services;
using Awacash.Domain.Entities;
using Awacash.Domain.Helpers;
using Awacash.Infrastructure;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration((config, context) =>
{

    context.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{config.HostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true)
               .AddEnvironmentVariables();
    MailTemplateHelper.Initialize(config.HostingEnvironment);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp",
        builder =>
        {
            builder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });

});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseInfrastructure(builder.Configuration);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

Audit.Core.Configuration.Setup()
    .UseEntityFramework(ef => ef
    //.UseDbContext<AppIdentityDbContext>(optionsBuilder.Options)
    .AuditTypeMapper(t => typeof(AuditLog))
    .AuditEntityAction<AuditLog>((evt, entry, audit) =>
    {
        string username = string.Empty, ipAddress = string.Empty;
        switch (entry.Action.ToLower())
        {
            case "insert":
                {
                    entry.ColumnValues.TryGetValue("CreatedBy", out object userObj);
                    username = userObj?.ToString() ?? "N/A";
                    entry.ColumnValues.TryGetValue("CreatedByIp", out object ipObj);
                    ipAddress = ipObj?.ToString() ?? "N/A";
                }
                break;
            case "update":
            case "delete":
                {
                    entry.ColumnValues.TryGetValue("ModifiedBy", out object userObj);
                    username = userObj?.ToString() ?? "N/A";
                    entry.ColumnValues.TryGetValue("ModifiedByIp", out object ipObj);
                    ipAddress = ipObj?.ToString() ?? "N/A";


                }
                break;
                //case "delete":
                //    {
                //        username = httpContextAccessor.HttpContext.User?.Identity.Name ?? "N/A";

                //        if (httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                //            ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //        else
                //            ipAddress = "N/A";
                //    }
                //    break;
        }

        if (username == "N/A")
            username = /*httpContextAccessor.HttpContext?.User?.Identity.Name ??*/ "N/A";

        //if (ipAddress == "N/A" && httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
        //    ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        //else
        ipAddress = "N/A";

        var changes = entry.Changes?.Where(x => x.NewValue != null && x.OriginalValue != null && !x.NewValue.Equals(x.OriginalValue)).ToList();

        if (entry.Action.ToLower() == "update" && (changes == null || changes?.Count == 0))
            return false;

        // Common action on AuditLog
        audit.EventDate = DateTime.Now;
        audit.EventType = entry.Action;
        audit.ColumnValues = JsonConvert.SerializeObject(entry.ColumnValues);
        audit.IPAddress = ipAddress;
        audit.UserName = username;
        audit.Changes = entry.Action.ToLower() == "update" ? JsonConvert.SerializeObject(changes) : "N/A";
        audit.TableName = entry.Table;
        audit.KeyValues = JsonConvert.SerializeObject(entry.PrimaryKey);
        audit.OldValues = "N/A";
        audit.NewValues = "N/A";
        audit.CreatedBy = "System";
        audit.CreatedDate = DateTime.Now;
        audit.CreatedByIp = "::1";

        return true;
    }).IgnoreMatchedProperties(true));

app.Run();
