using AutoMapper;
using IMCTaxService.Entities.AppObjects;
using IMCTaxService.Entities.AppSettings;
using IMCTaxService.Proxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMCTaxService
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
            services.AddControllers();

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1";
                document.ApiGroupNames = new[] { "1" };
                document.Title = "IMCTaxService";
                document.Description = "Please enjoy!";
                document.GenerateEnumMappingDescription = true;
            });

            services.AddApiVersioning(versioningOptions =>
            {
                versioningOptions.ReportApiVersions = true;
                versioningOptions.AssumeDefaultVersionWhenUnspecified = true;
                versioningOptions.DefaultApiVersion = new ApiVersion(1, 0);
                versioningOptions.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddMvc(mvcOptions =>
            {
                mvcOptions.Filters.Add<GlobalExceptionFilter>();
            });

            ConfigureDependencyInjection(services);
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.Configure<TaxJarSettings>(Configuration.GetSection("TaxJarSettings"));
            services.AddScoped<IJsonConvert, JsonConverter>();
            services.AddScoped<ITaxClient, TaxJarTaxClient>();
            services.AddScoped<IFactory<ITaxClient, UserType>, TaxClientFactory>();
            services.AddScoped<IRestClient>(provider =>
            {
                var taxJarOptions = provider.GetService<IOptions<TaxJarSettings>>();
                var taxJarSettings = taxJarOptions.Value;
                var restClient = new RestClient(taxJarSettings.ServiceUrl);
                restClient.AddDefaultHeader("Authorization", $"Bearer {taxJarSettings.ApiKey}");
                return restClient;
            });

            services.AddAutoMapper(typeof(AutoMapperServiceProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOpenApi();
            app.UseSwaggerUi3(uiSettings =>
            {
                uiSettings.Path = "/swagger";
            });
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
