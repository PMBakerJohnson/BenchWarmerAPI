using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using BenchWarmerAPI.Models;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;

namespace BenchWarmerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string PolicyName = "AngularLocalTestPolicy";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();
            services.AddODataQueryFilter();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<BenchwarmersContext>(options => options.UseSqlServer("Data Source=benchwarmersdb.crsp7d0nfbrs.us-east-2.rds.amazonaws.com,1521;Initial Catalog=Benchwarmers;User ID=admin;Password=gkTBC1grTnxWVI89GA2F;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                    });
            });     

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "BenchWarmerAPI", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(PolicyName);
            app.UseHttpsRedirection();
            app.UseMvc(builder => {
                builder.EnableDependencyInjection();
                builder.Expand().Select().OrderBy().Filter();
                }
            );
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BenchWarmerAPI V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
