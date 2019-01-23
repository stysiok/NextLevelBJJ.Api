using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.GraphQLClasses;
using NextLevelBJJ.DataService.Models;
using Microsoft.EntityFrameworkCore;
using NextLevelBJJ.DataServices;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.Api.Types;
using NextLevelBJJ.ScheduleService;
using NextLevelBJJ.WebContentServices.Abstraction;

namespace NextLevelBJJ.Api
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
            services.AddMvc();
            
            //Types
            services.AddSingleton<AttendanceType>();
            services.AddSingleton<ClassType>();
            services.AddSingleton<Types.PassType>();
            services.AddSingleton<PassTypeType>();
            services.AddSingleton<StudentType>();
            services.AddSingleton<TrainingDayType>();

            
            //Services & Database
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddDbContext<NextLevelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NextLevelDatabase")));
            services.AddTransient<ITrainingsService, TrainingsService>();

            
            
            //Main Graphql objs
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<GraphQLQuery>();
            services.AddTransient<NextLevelBJJQuery>();

            var sp = services.BuildServiceProvider();
            //Schema
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(type => sp.GetService(type)));
            services.AddSingleton<ISchema, NextLevelBJJSchema>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseGraphiQl();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
