using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Services;
using MonitorRedCore.Infraestructure.Data;
using MonitorRedCore.Infraestructure.Filters;
using MonitorRedCore.Infraestructure.Repositories;

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
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<MONITOREDContext>(options => options.UseSqlServer(Configuration["LocalConnectionString"]));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
