using System;
using System.IO;
using System.Reflection;
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
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MonitorRedCore.Core.CustomEntities;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Services;
using MonitorRedCore.Infraestructure.Data;
using MonitorRedCore.Infraestructure.Filters;
using MonitorRedCore.Infraestructure.Interfaces;
using MonitorRedCore.Infraestructure.Repositories;
using MonitorRedCore.Infraestructure.Services;

namespace MonitorRedCore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private AwsOptions AwsOptions { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCognitoIdentity();
            services.AddControllers(options => {
                options.Filters.Add<GlobalExceptionFilter>();
            })
                .AddNewtonsoftJson((options) =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<MONITOREDContext>(options => options.UseSqlServer(Configuration["LocalConnectionString"]));

            // Configures...
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

            //services.AddSingleton<IAwsService>(sp =>
            //{
            //    var awsService = sp.GetRequiredService<AwsService>().GetAwsOptions().Result;
            //    AwsOptions.UserPoolClientId = awsService.UserPoolClientId;
            //    AwsOptions.MetaDataUrl = awsService.MetaDataUrl;
            //    return new AwsService();
            //});

            // Options...
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IAwsService>(async (options, awsService) =>
                {
                    var awsOptions = await awsService.GetAwsOptions();

                    options.Audience = awsOptions.UserPoolClientId;
                    options.Authority = awsOptions.MetaDataUrl;
                });

            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "MonitorRed API", Version = "v1"});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                doc.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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
