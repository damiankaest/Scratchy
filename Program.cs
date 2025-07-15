using Microsoft.AspNetCore.Authentication;
using Scratchy.Initializer;
using Scratchy.Persistence.DB;

var builder = WebApplication.CreateBuilder(args);

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Services.ConfigureCors();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureMongoDB(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureApplicationInsights(builder.Configuration);

builder.Services.AddAuthentication("Firebase")
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>("Firebase", null);

builder.Services.AddAuthorization();

builder.Logging.AddConsole();

builder.Services.AddControllers();

var app = builder.Build();

// Initialize MongoDB indexes on startup
using (var scope = app.Services.CreateScope())
{
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Initializing MongoDB indexes...");
        await mongoContext.InitializeIndexesAsync();
        logger.LogInformation("MongoDB indexes initialized successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to initialize MongoDB indexes");
        // Don't stop the application, but log the error
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add health check endpoint
app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
