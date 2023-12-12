using Awacash.Application;
using Awacash.Domain.Helpers;
using Awacash.Infrastructure;
using Awacash.Infrastructure.Filters;
using Awacash.Infrastructure.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.ConfigureAppConfiguration((config, context) =>
{

    context.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{config.HostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true)
               .AddEnvironmentVariables();
    MailTemplateHelper.Initialize(config.HostingEnvironment);
});
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ApiAuthorizationActionFilter>();
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


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.UseSwagger();
//    app.UseSwaggerUI();
//}
//else
//{
//    //app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseMiddleware<RateLimitMiddleware>( 1);
app.UseInfrastructure(builder.Configuration);
//app.UseCors("corsapp");
//app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
JobStartup.AddJobs();
app.Run();
