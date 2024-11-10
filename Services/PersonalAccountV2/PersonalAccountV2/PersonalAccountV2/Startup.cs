﻿using AutoMapper;
using Microsoft.OpenApi.Models;
using PersonalAccountV2.Mapping;
using PersonalAccountV2.RabbitMq;

namespace PersonalAccountV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            InstallAutomapper(services);
            services.AddServices(Configuration);
            services.AddControllers();
            services.AddCors();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonalAccountV2Api", Version = "v1" });

            }); ;
            services.AddHostedService<RabbitMqListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            if (!env.IsProduction())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapControllers();
            });
        }

        private static IServiceCollection InstallAutomapper(IServiceCollection services)
        {
            services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
            return services;
        }

        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AccomplishmentMappingsProfile>();
                cfg.AddProfile<CommunicationMappingsProfile>();
                cfg.AddProfile<EmployeeMappingsProfile>();
                cfg.AddProfile<EventMappingsProfile>();
                cfg.AddProfile<ExperienceMappingsProfile>();
                cfg.AddProfile<SkillMappingsProfile>();
                cfg.AddProfile<BS.Services.Implementations.Mapping.AccomplishmentMappingsProfile>();
                cfg.AddProfile<BS.Services.Implementations.Mapping.CommunicationMappingsProfile>();
                cfg.AddProfile<BS.Services.Implementations.Mapping.EmployeeMappingsProfile>();
                cfg.AddProfile<BS.Services.Implementations.Mapping.EventMappingsProfile>();
                cfg.AddProfile<BS.Services.Implementations.Mapping.ExperienceMappingsProfile>();
                cfg.AddProfile<BS.Services.Implementations.Mapping.SkillMappingsProfile>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}
