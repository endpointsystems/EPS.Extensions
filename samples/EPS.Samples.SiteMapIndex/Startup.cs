using System.Linq;
using System.Text.Json;
using AspNetCore.RouteAnalyzer;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Newtonsoft.Json;

namespace EPS.Samples.SiteMapIndex
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
            // add response compression for the sitemaps. For more information:
            //https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-3.1
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddRazorPages();
            services.AddLazyCache();
            services.AddRouteAnalyzer();
            services.AddCarter();
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

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCarter();
                //endpoints.MapControllerRoute("SiteMap","{area:exists}/{controller=SiteMap/{fileName}")
                //endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapGet("/routes", request =>
                {
                    request.Response.Headers.Append("content-type", "application/json");

                    var ep = endpoints.DataSources.First().Endpoints.Select(e => e as RouteEndpoint);
                    return request.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            ep.Select(e => new
                            {
                                Method = ((HttpMethodMetadata)e.Metadata.First(m => m is HttpMethodMetadata)).HttpMethods.First(),
                                Route = e.RoutePattern.RawText
                            })
                        )
                    );
                });
            });
        }
    }
}
