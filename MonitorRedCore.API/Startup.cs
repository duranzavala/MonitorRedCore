using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.S3;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Services;
using MonitorRedCore.Infraestructure.Data;
using MonitorRedCore.Infraestructure.Filters;
using MonitorRedCore.Infraestructure.Interfaces;
using MonitorRedCore.Infraestructure.Repositories;
using MonitorRedCore.Infraestructure.Services;
using Newtonsoft.Json;

namespace MonitorRedCore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => {
                options.Filters.Add<GlobalExceptionFilter>();
            })
                .AddNewtonsoftJson((options) =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<MONITOREDContext>(options => options.UseSqlServer(Configuration["LocalConnectionString"]));

            // Options...
            services.Configure<AwsOptions>(Configuration.GetSection("AwsOptions"));
            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));

            // Services...
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());

                return new UriService(absoluteUri);
            });
            services.AddSingleton<IAwsService, AwsService>();

            var serviceProvider = services.BuildServiceProvider();
            var awsService = serviceProvider.GetRequiredService<IAwsService>();
            var awsOptions = awsService.GetAwsOptions().Result;

            CognitoUserPool cognitoUserPool = new CognitoUserPool(
                awsOptions.UserPoolId,
                awsOptions.UserPoolClientId,
                new AmazonCognitoIdentityProviderClient(),
                awsOptions.UserPoolClientIdSecret
            );

            services.AddSingleton(cognitoUserPool);
            services.AddCognitoIdentity();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Audience = awsOptions.UserPoolClientId;
                   // Supuestamente aqui busca los tokens generados
                   options.Authority = $"https://cognito-idp.{RegionEndpoint.USEast2}.amazonaws.com/{awsOptions.UserPoolId}";
                   options.RequireHttpsMetadata = false;

                   // otra opcion...
                   //options.TokenValidationParameters = new TokenValidationParameters
                   //{
                   //    IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                   //    {
                   //        // get JsonWebKeySet from AWS
                   //        var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                   //        // serialize the result
                   //        var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                   //        // cast the result to be the type expected by IssuerSigningKeyResolver
                   //        return (IEnumerable<SecurityKey>)keys;
                   //    },

                   //    ValidIssuer = "https://cognito-idp.{region}.amazonaws.com/{pool ID}",
                   //    ValidateIssuerSigningKey = true,
                   //    ValidateIssuer = true,
                   //    ValidateLifetime = true,
                   //    ValidAudience = "{Cognito AppClientID}",
                   //    ValidateAudience = true
                   //};
               });

            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "MonitorRed API", Version = "v1"});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                doc.IncludeXmlComments(xmlPath);
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MonitorRed API V1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
