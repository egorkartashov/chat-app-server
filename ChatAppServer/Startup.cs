using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAppServer.ClientHubs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ChatAppServer
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
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatAppServer", Version = "v1" });
			});

			services.AddSignalR();

			// services.AddCors(options =>
			// {
			// 	options.AddPolicy("AllowAllHeaders",
			// 		builder =>
			// 		{
			// 			builder.AllowAnyOrigin()
			// 				.AllowAnyHeader()
			// 				.AllowAnyMethod();
			// 		});
			// });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if (env.IsDevelopment())
			{
				app.UseHttpsRedirection();
				app.UseDeveloperExceptionPage();
				app.UseSwagger();			
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatAppServer v1"));
			}

			app.UseRouting();

			app.UseCors(options =>
			{
				options.AllowAnyHeader();
				options.AllowAnyMethod();
				options.WithOrigins("http://localhost:4200");
				options.AllowCredentials();
			});

			// app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<UsersHub>("/signalr");
			});
		}
	}
}