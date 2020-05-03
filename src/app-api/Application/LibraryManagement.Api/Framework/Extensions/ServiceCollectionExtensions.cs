using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagement.Application;
using LibraryManagement.Core;
using LibraryManagement.Core.Configuration;
using LibraryManagement.Core.Domain.Model;
using LibraryManagement.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LibraryManagement.Api.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddLogging();
            services.AddControllers();

            services.AddMediatR(typeof(MessageResponse<>).Assembly);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials());
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management API", Version = "v1" });
                cfg.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //cfg.IncludeXmlComments(xmlPath);
            });

        }

        /// <summary>
        ///     Configure jwt authentication
        /// </summary>
        /// <param name="services">current</param>
        /// <param name="appConfig">App config.</param>
        public static void AddJwtAuthentication(this IServiceCollection services, ILibraryManagementConfig appConfig)
        {
            var key = Encoding.ASCII.GetBytes(appConfig.AppSecret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                            var user = userRepository.GetById(Guid.Parse(context.Principal.Identity.Name));
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static void AddPersistenceServices(this IServiceCollection services, ILibraryManagementConfig appConfig)
        {
            services.AddAutoMapper();

            services.AddDbContext<LibraryManagementObjectContext>(optionsBuilder =>
            {

                if (appConfig.IsTesting)
                    optionsBuilder.UseLazyLoadingProxies()
                        .UseInMemoryDatabase("LibraryManagementTestDb");
                else
                    // production Db
                    optionsBuilder.UseLazyLoadingProxies()
                        .UseSqlServer(appConfig.DataConnectionString,
                            options => { options.MigrationsAssembly("LibraryManagement.Infrastructure"); });
            });


        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            var instances = typeof(LibraryManagementObjectContext).Assembly
                .GetTypes()
                .Where(type => typeof(Profile).IsAssignableFrom(type))
                .Select(mapperConfiguration => (Profile)Activator.CreateInstance(mapperConfiguration));

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    if (instance != null)
                        cfg.AddProfile(instance.GetType());
                }
            });

            //register
            Singleton<IMapper>.Instance = config.CreateMapper();
        }
    }
}
