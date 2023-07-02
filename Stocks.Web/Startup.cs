using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocks.Core.Cache;
using Stocks.Core.Excel;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Domain.Models;
using Stocks.Web.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Web
{
    public class StocksRepositoryFake : IStocksRepository
    {
        public async Task<IEnumerable<StockDividend>> GetStocksAsync()
        {
            return Array.Empty<StockDividend>();
        }
    }

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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ICachedRepository, RedisCachedRepository>();
            services.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
            services.AddTransient<IStocksLoader, StocksLoader>();
            services.AddTransient<IStocksOfInterestRespository, StocksOfInterestRespository>();
            services.AddTransient<IStocksRepository, StocksRepositoryFake>();
            services.Decorate<IStocksRepository, StocksRepositoryCachingDecorator>();
            services.AddTransient<IStockExcelWriter, StockExcelWriter>();
            services.AddTransient<IExcelSaver, ExcelSaver>();
            services.AddTransient<IDividendByMonthCollectionPreparer, DividendByMonthCollectionPreparer>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
