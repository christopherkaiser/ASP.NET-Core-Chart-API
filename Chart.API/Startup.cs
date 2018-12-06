using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chart.API.Entities;
using Chart.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chart.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                   .AddMvcOptions(o => o.OutputFormatters.Add(
                       new XmlDataContractSerializerOutputFormatter()));

            //var connectionString = Startup.Configuration["connectionStrings:chartsDBConnectionString"];
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=ChartDB;Trusted_Connection=True;";
            services.AddDbContext<ChartContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IChartRepository, ChartRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ChartContext chartContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Chart, Models.ChartDto>();
                cfg.CreateMap<Models.ChartForCreationDto, Entities.Chart>();
                cfg.CreateMap<Models.ChartForUpdateDto, Entities.Chart>();
                cfg.CreateMap<Entities.Chart, Models.ChartForUpdateDto>();
                cfg.CreateMap<Entities.ValuePair, Models.ValuePairDto>();
                cfg.CreateMap<Models.ValuePairForCreationDto, Entities.ValuePair>();
                cfg.CreateMap<Models.ValuePairForUpdateDto, Entities.ValuePair>();
                cfg.CreateMap<Entities.ValuePair, Models.ValuePairForUpdateDto>();
            });

            app.UseMvc();
        }
    }
}
