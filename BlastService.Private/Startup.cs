using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using BlastService.Private.Models;

using Newtonsoft.Json.Converters;
using NetTopologySuite.Geometries;
using Npgsql;

namespace BlastService.Private
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
            NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite(handleOrdinates: Ordinates.XYZ);

            services.AddDbContext<ProjectContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("Context"), o => o.UseNetTopologySuite());
                opt.UseLazyLoadingProxies(false);
            }) ;

            // Globally convert all enums to strings when serializing to json
            services.AddControllers().AddNewtonsoftJson(opts =>
                opts.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v0.3.0";
                    document.Info.Title = "Blast Service Private API";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
