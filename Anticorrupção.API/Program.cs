using Anticorrup��o.API.Models;
using Anticorrup��o.API.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura��o
var configuration = builder.Configuration;

// Servi�os Cont�iner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IGitHubService, GitHubService>();

// Op��es do token do GitHub
var gitHubTokenOptions = builder.Configuration.GetSection("GitHubTokenOptions").Get<GitHubTokenOptions>();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AntiCorrup��o API", Version = "v1" });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var options = serviceProvider.GetRequiredService<IOptions<GitHubTokenOptions>>();
    options.Value.AccessToken = gitHubTokenOptions.AccessToken;
}


// Configura��o da Pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AntiCorrup��o API v1");
        //c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
