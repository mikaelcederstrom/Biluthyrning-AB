using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.Repositorys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Biluthyrning_AB
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BiluthyrningContext>(o => o.UseSqlServer(connString));

            services.AddTransient<CarsService>();
            services.AddTransient<CustomersService>();
            services.AddTransient<EventsService>();
            services.AddTransient<OrdersService>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<ICarsRepository, CarsRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();
        }
    }
}
