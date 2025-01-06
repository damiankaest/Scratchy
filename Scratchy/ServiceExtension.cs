using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scratchy.Application.Services;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Persistence.DB;
using Scratchy.Persistence.Repositories;
using Scratchy.Services;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Scratchy API",
                Version = "v1"
            });
        });
    }

    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ScratchItDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IScratchRepository, ScratchRepository>();
        services.AddTransient<IAlbumRepository, AlbumRepository>();
        services.AddTransient<ILibraryRepository, LibraryRepository>();
        services.AddTransient<IArtistRepository, ArtistRepository>();
        services.AddTransient<IFriendshipRepository, FriendshipRepository>();
        services.AddTransient<IFollowRepository, FollowRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IBlobService, BlobService>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            return new BlobService(configuration.GetConnectionString("BlobStorageConnectionString"));
        });
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ISpotifyService, SpotifyService>();
        services.AddScoped<ILibraryService, LibraryService>();
        services.AddScoped<IExplorerService, ExploreService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFriendshipService, FriendshipService>();
        services.AddScoped<IFollowerService, FollowService>();
        services.AddScoped<INotificationService, NotificationService>();
    }

    public static void ConfigureApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
        {
            ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
        });
    }

    //public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    //{
    //    var firebaseProjectId = configuration["Firebase:ProjectId"]; // Firebase-Projekt-ID

    //    services.AddAuthentication(options =>
    //    {
    //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //    })
    //    .AddJwtBearer(options =>
    //    {

    //        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
    //        options.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuer = true,
    //            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
    //            ValidateAudience = true,
    //            ValidAudience = firebaseProjectId,
    //            ValidateLifetime = true,
    //            ValidateIssuerSigningKey = true
    //        };

    //        options.Events = new JwtBearerEvents
    //        {
    //            OnAuthenticationFailed = context =>
    //            {
    //                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
    //                return Task.CompletedTask;
    //            },
    //            OnTokenValidated = async context =>
    //            {
    //                try
    //                {
    //                    var idToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

    //                    var firebaseToken = await FirebaseWrapper.VerifyTokenAsync(idToken);
    //                    Console.WriteLine($"Token validated successfully for user: {firebaseToken.Uid}");

    //                    var claimsIdentity = context.Principal.Identity as System.Security.Claims.ClaimsIdentity;
    //                    if (claimsIdentity != null)
    //                    {
    //                        claimsIdentity.AddClaim(new System.Security.Claims.Claim("user_id", firebaseToken.Uid));
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    Console.WriteLine($"Custom Firebase validation failed: {ex.Message}");
    //                    throw;
    //                }
    //            }
    //        };
    //    });
    //}
}
