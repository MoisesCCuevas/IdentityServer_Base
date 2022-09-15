// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AuthenticationSystem.Data;
using Microsoft.EntityFrameworkCore;
using AuthenticationSystem.Quickstart;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationSystem
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var AllowOrigins = Configuration.GetSection("AllowOrigins").Get<string[]>();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                cors =>
                {
                    cors.AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithOrigins(AllowOrigins)
                           .AllowAnyOrigin()
                           .AllowCredentials();
                }));

            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Users")));

            var builder = services.AddIdentityServer().AddProfileService<ProfileService>();

            // in-memory, code config
            builder.AddInMemoryIdentityResources(Config.GetIdentityResources());
            builder.AddInMemoryApiResources(Config.GetAllApiResources());
            builder.AddInMemoryClients(Config.GetClients());

            services.AddSingleton<Encrypt>();
            services.AddScoped<IUserStore, UserStore>();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}