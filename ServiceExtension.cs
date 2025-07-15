using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Scratchy.Application.Services;
using Scratchy.Domain.Configuration;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Persistence.DB;
using Scratchy.Persistence.HealthChecks;
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

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // Register the generic MongoDB repository
        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
        
        // Register specific repositories that extend the base MongoDB repository
        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IScratchRepository, ScratchRepository>();
        // services.AddScoped<IAlbumRepository, AlbumRepository>();
        // services.AddScoped<ILibraryRepository, LibraryRepository>();
        // services.AddScoped<IArtistRepository, ArtistRepository>();
        // services.AddScoped<IFollowRepository, FollowRepository>();
        // services.AddScoped<INotificationRepository, NotificationRepository>();
        // services.AddScoped<IPostRepository, PostRepository>();
        // services.AddScoped<IBadgeRepository, BadgeRepository>();
        // services.AddScoped<ITrackRepository, TrackRepository>();
    }

    public static void ConfigureMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure MongoDB settings
        services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
        
        // Register MongoDB client as singleton (MongoDB client is thread-safe)
        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        // Register MongoDB database as scoped
        services.AddScoped<IMongoDatabase>(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            return client.GetDatabase(settings.DatabaseName);
        });
        
        // Register MongoDB context as scoped - explicit registration with dependencies
        services.AddScoped<MongoDbContext>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>();
            var logger = serviceProvider.GetRequiredService<ILogger<MongoDbContext>>();
            return new MongoDbContext(settings, logger);
        });
        
        // Register health check for MongoDB
        services.AddHealthChecks()
            .AddCheck<MongoDbHealthCheck>("mongodb");
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
        services.AddScoped<IFollowService, FollowService>();
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
