using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Endpoints;
using ImaginedWorlds.Infrastructure;
using ImaginedWorlds.Infrastructure.Persistence;
using ImaginedWorlds.Infrastructure.Specialists;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration));


    const string allowedOriginsPolicy = "AllowedOrigins";

    builder.Services.AddSingleton<IPromptManager, PromptManager>();
    builder.Services.AddSingleton<ISimulationNotifier, SignalRNotifier>();
    builder.Services.AddSingleton<ISecretVault, ConfigurationSecretVault>();

    builder.Services.AddScoped<IPromptBuilder, PromptBuilder>();
    builder.Services.AddScoped<IRequestFactory, RequestFactory>();
    builder.Services.AddScoped<IArchitectFactory, ArchitectFactory>();
    builder.Services.AddScoped<IAgentRepository, AgentRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddTransient<IFocuser, Focuser>();
    builder.Services.AddTransient<IExecutor, Executor>();
    builder.Services.AddTransient<ICoordinator, Coordinator>();

    builder.Services.AddHttpClient("llm-client", client =>
    {
        client.Timeout = TimeSpan.FromSeconds(120);
    });

    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddMediator(options =>
    {
        options.ServiceLifetime = ServiceLifetime.Scoped;
    });
    builder.Services.AddSignalR();

    var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:3000";
    string[] frontendUrls = [
        frontendUrl,
    "https://localhost:3000",
    "http://localhost:3000",
    "ws://localhost:3000",
    "wss://localhost:3000"
    ];

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: allowedOriginsPolicy, policy =>
        {
            policy.WithOrigins(frontendUrls)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    var webSocketOptions = new WebSocketOptions
    {
        KeepAliveInterval = TimeSpan.FromMinutes(2)
    };

    foreach (string url in frontendUrls)
    {
        webSocketOptions.AllowedOrigins.Add(url);
    }

    builder.Services.AddWebSockets(op =>
    {
        foreach (string url in frontendUrls)
        {
            op.AllowedOrigins.Add(url);
        }
    });

    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseRouting();


    app.UseWebSockets(webSocketOptions);

    app.UseCors(allowedOriginsPolicy);

    app.MapAgentEndpoints();
    app.MapCreationEndpoints();

    app.MapHub<ImaginedWorldsHub>("/imaginedWorldsHub");

    //app.MapHealthChecks("/_health");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}