using Api.Services;
using Api.Configs;
using Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Api.Mapper;
using AspNetCoreRateLimit;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var authSection = builder.Configuration.GetSection(AuthConfig.Position);
        var authConfig = authSection.Get<AuthConfig>();

        builder.Services.Configure<AuthConfig>(authSection);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "Введите токен пользователя",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,
                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            // Генерация Swagger документа
            c.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth" });
            c.SwaggerDoc("Api", new OpenApiInfo { Title = "Api" });
        });

        builder.Services.AddDbContext<DAL.DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), sql => { });
        }, contextLifetime: ServiceLifetime.Scoped);

        builder.Services.AddOptions();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
        builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
        builder.Services.AddInMemoryRateLimiting();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<PostService>();
        builder.Services.AddScoped<LinkGeneratorService>();
        builder.Services.AddScoped<CommentService>();
        builder.Services.AddScoped<LikeService>();

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false; // Поставить в true в действующем приложении
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = authConfig.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authConfig.SymmetricSecurityKey(),
                ClockSkew = TimeSpan.Zero,
            };
            //o.SecurityTokenValidators.Add(new TokenValidator());
        });

        builder.Services.AddAuthorization(o =>
        {
            o.AddPolicy("ValidAccessToken", p =>
            {
                p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                p.RequireAuthenticatedUser();
            });
        });



        var app = builder.Build();

        using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if(serviceScope != null)
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DAL.DataContext>();
                context.Database.Migrate();
            }
        }

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            //Говорим интерфейсу про два EndPoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("Api/swagger.json", "Api");
                c.SwaggerEndpoint("Auth/swagger.json", "Auth");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseClientRateLimiting();
        app.UseAuthorization();
        app.UseTokenValidator();
        app.UseGlobalErrorWrapper();
        app.MapControllers();
        app.Run();
    }
}