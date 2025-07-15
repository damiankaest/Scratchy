using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Scratchy.Application.Services;
using Scratchy.Domain.Configuration;
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
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        // MongoDB repositories
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IScratchRepository, ScratchRepository>();
        services.AddTransient<IAlbumRepository, AlbumRepository>();
        services.AddTransient<ILibraryRepository, LibraryRepository>();
        services.AddTransient<IArtistRepository, ArtistRepository>();
        services.AddTransient<IFriendshipRepository, FriendshipRepository>();
        services.AddTransient<IFollowerRepository, FollowRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();
        services.AddTransient<IBadgeRepository, BadgeRepository>();
        services.AddTransient<IUserBadgeRepository,UserBadgeRepository>();
        services.AddTransient<IShowCaseRepository, ShowCaseRepository>();
        services.AddTransient<ITrackRepository, TrackRepository>();
        services.AddTransient<IPostRepository, PostRepository>();
    }

    public static void ConfigureMongoDB(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure MongoDB settings from appsettings.json
        services.Configure<MongoDBSettings>(configuration.GetSection(MongoDBSettings.SectionName));
        
        // Register MongoDB context as singleton
        services.AddSingleton<MongoDbContext>();
        
        // Register generic MongoDB repository
        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
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
        services.AddSingleton<ISpotifyService,SpotifyService>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            return new SpotifyService("2783d9fdaf5848ab9a4b7248215f5844", "02d9765ad365414097f1ed7924f71241");
            //return new SpotifyService(
            //    configuration.GetValue<string>("Spotify:ClientId"), 
            //    configuration.GetValue<string>("Spotify:ClientSecret"));
        });

        services.AddScoped<ILibraryService, LibraryService>();
        services.AddScoped<IExplorerService, ExploreService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFriendshipService, FriendshipService>();
        services.AddScoped<IFollowerService, FollowerService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IBadgeService, BadgeService>();
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<IScratchService, ScratchService>();
        services.AddScoped<IStatService, StatService>();
        services.AddScoped<IShowCaseService, ShowCaseService>();
        services.AddScoped<ICollectionService, CollectionService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddMemoryCache();
    }

    public static void ConfigureApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
        {
            ConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
        });
    }
}
