using ImaginedWorlds.Application.Abstractions;
using ImaginedWorlds.Domain.Agent;
using ImaginedWorlds.Endpoints;
using ImaginedWorlds.Infrastructure;
using ImaginedWorlds.Infrastructure.Persistence;
using ImaginedWorlds.Infrastructure.Specialists;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddMediator();

var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
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

app.UseCors();

app.MapAgentEndpoints();
app.MapCreationEndpoints();

app.MapHub<ImaginedWorldsHub>("/imaginedWorldsHub");

app.MapHealthChecks("/_health");

app.Run();
