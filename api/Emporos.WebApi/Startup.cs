using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Coravel;
using FluentValidation;
using FluentValidation.AspNetCore;
using Emporos.Core.Dto;
using Emporos.Core.Profiles;
using Emporos.Core.Stores;
using Emporos.WebApi.Validators;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Emporos.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(m => m.AddPolicy("CorsPolicy", c => 
                c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            }).AddFluentValidation();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Emporos.API", Version = "v1"});
            });

            services.AddMediatR(typeof(ContactRequestDto));
            services.AddScheduler();
            services
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(
                    (sp, options) => { options.UseInMemoryDatabase($"ContactDB").UseInternalServiceProvider(sp); },
                    ServiceLifetime.Singleton);

            services.AddSingleton(new MapperConfiguration(m => m.AddProfile<MappingProfile>()).CreateMapper());
            services.AddTransient<IValidator<ContactRequestDto>, UpdateContactValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Emporos.API v1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}