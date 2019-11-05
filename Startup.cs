using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using apiaccounts.Services;
using apiaccounts.Services.Interfaces;

namespace apiaccounts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var allowedOrigins = Configuration.GetSection("CORS-Settings:Allow-Origins")
                .Get<string[]>();

            var allowedHeaders = Configuration.GetSection("CORS-Settings:Allow-Headers")
                .Get<string[]>();

            var allowedMethods = Configuration.GetSection("CORS-Settings:Allow-Methods")
                .Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .WithOrigins(allowedOrigins)
                            .WithHeaders(allowedHeaders)
                            .WithMethods(allowedMethods)
                            .AllowCredentials();
                    });
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"]))
                    };
                });

            //injecting Settings in the Options accessor model
            services.Configure<Settings>(Options => {
                Options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                Options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthService, AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(MyAllowSpecificOrigins);
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
