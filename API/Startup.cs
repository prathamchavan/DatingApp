using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        
        public Startup(IConfiguration config)
        {
            _config = config;
            
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Here er check if we are in developement mode or not.
            //And if we are and our application encounters a problem
            //then we use the developer exception page.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            //if we did come in on HTTP address, then we get directed to the HTTPS endpoints.

            app.UseHttpsRedirection();

            //due to routing we are able to go through our browser and get to our weatherforcast controller

            app.UseRouting();

            app.UseAuthorization();

            //we have the middleware to actually use the endpoint

            app.UseEndpoints(endpoints =>
            {
                //method to map the controllers();
                endpoints.MapControllers();
            });
        }
    }
}
