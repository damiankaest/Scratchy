using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Scratchy.Initializer;

var builder = WebApplication.CreateBuilder(args);

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue; // Anzahl der Formulardatenfelder
    options.MultipartBodyLengthLimit = 104857600; // Maximale Gr��e f�r die Datei (100MB)
});

builder.Services.ConfigureCors();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureApplicationInsights(builder.Configuration);
builder.Services.AddAuthentication("Firebase")
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>("Firebase", null);

builder.Services.AddAuthorization();

builder.Logging.AddConsole();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
