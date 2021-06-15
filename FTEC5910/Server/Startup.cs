using FTEC5910.Server.Data;
using FTEC5910.Server.Services;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FTEC5910.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {            
            var dbSettings = Configuration.GetSection("CosmosDBConnection");
            var jwtSettings = Configuration.GetSection("JWTSettings");
            var audienceList = new List<string>();
            jwtSettings.GetSection("ValidAudiences").Bind(audienceList);

            services.AddDbContext<DataContext>(o => o.UseCosmos(dbSettings["Endpoint"],
                dbSettings["Key"],
                dbSettings["DatabaseName"]));
            //services.AddDbContext<DataContext>(o => o.UseSqlite("Data Source=DBFileName.db"));

            services.AddIdentity<MyIdentityUser, IdentityRole>().AddUserStore<MyUserStore>()
                    .AddEntityFrameworkStores<DataContext>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["ValidIssuer"],
                    ValidAudiences = audienceList,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecurityKey"]))
                };
            });
            services.AddScoped<DataService>();
            //.AddCookie();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                        
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                //Resolve ASP .NET Core Identity with DI help
                //var userManager = (UserManager<MyIdentityUser>)scope.ServiceProvider.GetService(typeof(UserManager<MyIdentityUser>));
                //var user = userManager.FindByNameAsync("C002").GetAwaiter().GetResult();
                // do you things here
                Debug.WriteLine($"Start Initialize Data {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                Console.WriteLine($"Start Initialize Data {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                scope.ServiceProvider.GetService<DataService>().InitializeData().GetAwaiter().GetResult();
                Debug.WriteLine($"Finish Initialize Data {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                Console.WriteLine($"Finish Initialize Data {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            };

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
