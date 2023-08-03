using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocks.Core;
using Stocks.Core.Cache;
using Stocks.Core.DividendDisplay;
using Stocks.Core.Loaders;
using Stocks.Core.Repositories;
using Stocks.Dal;
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
            services.AddSingleton<ICachedRepository, RedisCachedRepository>();
            services.AddTransient<IStockDividendHistoryLoader, StockDividendHistoryLoader>();
            services.AddTransient<IStocksLoader, StocksLoader>();
            services.AddTransient<IStocksOfInterestRespository, StocksOfInterestRespository>();
            services.AddTransient<IStocksRepository, StocksRepository>();
            services.Decorate<IStocksRepository, StocksRepositoryCachingDecorator>();
            services.AddTransient<IDividendByMonthCollectionPreparer, DividendByMonthCollectionPreparer>();
            services.AddTransient<ICalendarGenerator, CalendarGenerator>();
            services.AddTransient<IDateProvider, DateProvider>();
            services.AddDbContext<StockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StockWebDividendDB")));
            services.AddOptions<Settings>().Bind(Configuration.GetSection(Settings.SectionName));
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
