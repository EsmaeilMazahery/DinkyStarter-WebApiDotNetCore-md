using Dinky.Services.service;
using Dinky.WebApi.Extension;
using Dinky.WebApi.Extension.ActionFilters;
using Dinky.WebApi.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using StructureMap;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Dinky.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services = AddOAuthProviders(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
            }));

            //services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials();
            //}));

            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelStateCheckActionFilter());
            }).AddControllersAsServices();

            return ConfigureIoC(services);
        }

        public IServiceCollection AddOAuthProviders(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notifications")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = Task.Run(() => { return userService.SELECTAsync(userId); }).Result;
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    },
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

            });


            // services.AddAuthentication()
            //     .AddFacebook(o =>
            //     {
            //         o.AppId = this.Configuration["Authentication:Facebook:AppId"];
            //         o.AppSecret = this.Configuration["Authentication:Facebook:AppSecret"];
            //     });

            // services.AddAuthentication()
            //     .AddGoogle(o =>
            //     {
            //         o.ClientId = this.Configuration["Authentication:Google:ClientId"];
            //         o.ClientSecret = this.Configuration["Authentication:Google:ClientSecret"];
            //     });
            // services.AddAuthentication()
            //     .AddTwitter(o =>
            //     {
            //         o.ConsumerKey = this.Configuration["Authentication:Twitter:ConsumerKey"];
            //         o.ConsumerSecret = this.Configuration["Authentication:Twitter:ConsumerSecret"];
            //     });

            // services.AddAuthentication()
            //     .AddMicrosoftAccount(o =>
            //     {
            //         o.ClientId = this.Configuration["Authentication:Microsoft:ClientId"];
            //         o.ClientSecret = this.Configuration["Authentication:Microsoft:ClientSecret"];
            //     });


            return services;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //   app.UseOptions();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            //app.UseCors(builder => builder
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials()
            //);


            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationsHub>("/notifications");
            });
            app.UseMvc();

            //using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
            //}
        }

        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                // // Register stuff in container, using the StructureMap APIs...
                // config.Scan(_ =>
                // {
                //     _.AssemblyContainingType(typeof(Startup));
                //     _.WithDefaultConventions();
                //     //_.AddAllTypesOf<IGamingService>();
                //     _.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                // });

                // //config.For(typeof(IValidator<>)).Add(typeof(DefaultValidator<>));
                // //config.For(typeof(ILeaderboard<>)).Use(typeof(Leaderboard<>));
                // config.For<IUnitOfWork>().Use(_ => new DatabaseContext()).ContainerScoped();

                config.AddRegistry(new DefaultRegistry(Configuration.GetConnectionString("Default")));

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
