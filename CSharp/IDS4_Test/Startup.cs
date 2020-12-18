using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IDS4_Test
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

            //X509Certificate2 signingCert = new X509Certificate2("devcert.pfx", "123456");
            //X509SecurityKey privateKey = new X509SecurityKey(signingCert);
            //var credential = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256Signature);
            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtBearer:SecurityKey"]));
            //var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            services.AddIdentityServer()
                //.AddSigningCredential(credential)
                .AddDeveloperSigningCredential()
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.Authority = "http://localhost:5000/";
                     options.RequireHttpsMetadata = false;
                     
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         // ValidateIssuerSigningKey = true,
                         ValidateAudience = false,
                         //ValidateIssuerSigningKey=false
                         //IssuerSigningKey = new X509SecurityKey(new System.Security.Cryptography.X509Certificates.X509Certificate2())
                     };
                     IdentityModelEventSource.ShowPII = true;
                     options.MetadataAddress = "http://localhost:5000/.well-known/openid-configuration";//
                     options.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration();
                     //options.Audience = Config.ImageMan;
                     //options.MetadataAddress = "";
                 }).AddCookie();
          
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiScope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", Config.ImageMan);
            //    });
            //});
            //services.AddSingleton<ICorsPolicyService>((container) =>
            //{
            //    {
            //        var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
            //        return new DefaultCorsPolicyService(logger)
            //        {
            //            AllowAll = true
            //        };
            //    };
            //});
            services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ToDo API",
                Description = "A simple example ASP.NET Core Web API",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Shayne Boyer",
                    Email = string.Empty,
                    Url = new Uri("https://twitter.com/spboyer"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://example.com/license"),
                }
            });
            //Bearer ��scheme����
            var securityScheme = new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                //���������ͷ��
                In = ParameterLocation.Header,
                //ʹ��Authorizeͷ��
                Type = SecuritySchemeType.Http,
                //����Ϊ�� bearer��ͷ
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
            //�����з�������Ϊ����bearerͷ����Ϣ
            var securityRequirement = new OpenApiSecurityRequirement
                {
                        {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "bearerAuth"
                                    }
                                },
                                new string[] {}
                        }
                };
            //ע�ᵽswagger��
            c.AddSecurityDefinition("bearerAuth", securityScheme);
            c.AddSecurityRequirement(securityRequirement);
        });
            
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            //app.UseHttpCacheHeaders();


            app.UseRouting();

            app.UseIdentityServer();

            //app.UseHttpsRedirection();


            //app.UseCors();

            //app.UseAuthentication();
            app.UseAuthorization();




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
