using Awacash.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using TransactionQueryJob;
using TransactionQueryJob.Data;
using TransactionQueryJob.Interfaces;
using TransactionQueryJob.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Wema Transaction look up service";
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        services.AddTransient<AppDbContext>(x => new AppDbContext(optionBuilder.Options));


        MailTemplateHelper.Initialize(hostContext.HostingEnvironment);
        services.AddSingleton<IRestSharpHelper, RestSharpHelper>();
        services.AddTransient<ICommunicationService, CommunicationService>();
        services.AddHostedService<Worker>();
        services.AddHostedService<AccountOpeningWorker>();
    })
    .Build();

await host.RunAsync();
