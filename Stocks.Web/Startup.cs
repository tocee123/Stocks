using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocks.Core.Cache;
using Stocks.Core.Excel;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Web.Data;

namespace Stocks.Web
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ICachedRepository, MemoryCashedRepository>();
            services.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
            services.AddTransient<IStocksLoader, StocksLoader>();
            services.AddTransient<IStocksOfInterestRespository, StocksOfInterestRespository>();
            services.AddTransient<IStocksRepository>(sp =>
            {
                var stocksRepository =  new StocksRepository(sp.GetService<IStocksLoader>(), sp.GetService<IStocksOfInterestRespository>());
                var stockRepositoryDecorator = new StocksRepositoryCachingDecorator(stocksRepository, sp.GetService<ICachedRepository>());
                return stockRepositoryDecorator;
            });
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
