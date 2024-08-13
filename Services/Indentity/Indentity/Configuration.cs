using System.Text;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Context;
using DataAccess.Entities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Indentity;

public class Configuration
{
    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>() { new ApiScope("CssWebAPI", "Web API") };

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>()
    {
        new ApiResource("CssWebAPI", "Web API", new[] { JwtClaimTypes.Name })
        {
            Scopes = { "CssWebAPI" }
        }
    };

    public static IEnumerable<Client> Clients => new List<Client>()
    {
        new Client()
        {
            ClientId = "css-web-api",
            ClientName = "Css Web",
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = true,
            RedirectUris =
            {
                "http://localhost:5169/signin-oidc" //TODO Добавить редирект после аутентификации
            },
            AllowedCorsOrigins =
            {
                "http://localhost:5169" //TODO Добавить кому позволено использовать приложение
            },
            PostLogoutRedirectUris =
            {
                "http://localhost:5169/signout-oidc" //TODO Добавить редирект после логаута
            },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "CssWebAPI"
            },
            AllowAccessTokensViaBrowser = true
        }
    };
    
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CssWebApi", Version = "v1" });

            // Настройка схемы безопасности для JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }});
        });
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var secret = configuration.GetSection("secret").Value;
        
        services.AddDbContext<AuthContext>(options => options.UseNpgsql(connectionString));
        services.AddIdentity<User, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 6;
                config.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<AuthContext>()
            .AddDefaultTokenProviders();
    
        services.AddIdentityServer()
            .AddAspNetIdentity<User>()
            .AddInMemoryApiResources(Configuration.ApiResources)
            .AddInMemoryIdentityResources(Configuration.IdentityResources)
            .AddInMemoryApiScopes(Configuration.ApiScopes)
            .AddInMemoryClients(Configuration.Clients)
            .AddDeveloperSigningCredential();
    
        services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = "Css.Identity.Cookie";
            config.LoginPath = "/login";
            config.LogoutPath = "/logout";
        });
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("Bearer", options =>
        {
            options.Authority = "http://localhost:5169/";
            options.Audience = "CssWebAPI";
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:5169",
                ValidAudience = "CssWebAPI",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret??""))
            };
        });
        services.AddScoped<IUserService, UserService>();
    }
    
    
    public static void ConfigureMiddleware(WebApplication app)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    
        app.UseHttpsRedirection();
    
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context = serviceProvider.GetRequiredService<AuthContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception e)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error occurred while app initialization");
                throw;
            }
        }
        app.MapControllers();
    }
}