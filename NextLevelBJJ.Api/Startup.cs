﻿using System;
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
using AutoMapper;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.ScheduleService.Models;
using System.Globalization;
using NextLevelBJJ.WebContentServices;

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
            services.AddTransient<IPassesService, PassesService>();
            services.AddTransient<IPassTypesService, PassTypesService>();
            services.AddTransient<IAttendancesService, AttendancesService>();

            services.AddDbContext<NextLevelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NextLevelDatabase")));
            services.AddTransient<ITrainingsService, TrainingsService>();
            services.AddTransient<IClassesService, ClassesService>();
            
            //Main Graphql objs
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<GraphQLQuery>();
            services.AddTransient<NextLevelBJJQuery>();
            services.AddSingleton<IMapper>(MapperConfiguration().CreateMapper());

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

        private AutoMapper.IConfigurationProvider MapperConfiguration()
        {
            var culture = new CultureInfo("pl-PL");

            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Attendance, AttendanceDto>();

                cfg.CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.Day, 
                    opts => opts.MapFrom(src => culture.DateTimeFormat.GetDayName(src.Day).ToString()));

                cfg.CreateMap<Pass, PassDto>();

                cfg.CreateMap<DataService.Models.PassType, PassTypeDto>();

                cfg.CreateMap<Student, StudentDto>();

                cfg.CreateMap<TrainingDay, TrainingDayDto>()
                .ForMember(dest => dest.Day, 
                    opts => opts.MapFrom(src => culture.DateTimeFormat.GetDayName(src.Day).ToString()));
            });
        } 
    }
}
