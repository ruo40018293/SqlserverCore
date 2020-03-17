using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Common.AutoMapper;
using Common.Core;
using Common.Extension.MiddleWareExtension;
using Common.IOC;
using DAL;
using log4net;
using log4net.Config;
using log4net.Repository;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace WebApplication1
{
    public class Startup
    {
        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigHelper.Configs = configuration;
            //创建仓储名字随意
            repository = LogManager.CreateRepository("NetCoreRepository");

            //加载log4配置文件
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            InitRepository.loggerRepository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //添加过滤器
            services.AddMvc(o => o.Filters.Add(typeof(GlobalExceptions))).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());

            //注入EF上下文，注意生命周期是Scoped,即在同一个作用域中访问到的EF上下文为同一个，便于使用事务
            services.AddDbContext<EFDbcontext>(x => x.UseSqlServer(ConfigHelper.GetValue<string>("ConnectionsStrings:Development")), ServiceLifetime.Scoped);

            //注入Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "WebApplication1"
                });
                options.ResolveConflictingActions(x => x.First());
                var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "WebApplication1.xml");
                options.IncludeXmlComments(xmlPath);
            });

            //自动注入BLL下面的所有继承IDependency接口的类，
            var totalAssembly = new[]
            {
                Assembly.Load("BLL")
            };
            services.RegisterAssembliesTransient(totalAssembly);

            DIHelper.ServiceProvider = services.BuildServiceProvider();

            //添加AutoMapper映射关系
            services.AddAutoMapper(MapperRegister.MapType());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //配置自定义中间件
            app.UseHandleRequest();

            //配置可访问静态文件
            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1");
            });

            //配置AutoMapper对象映射通用方法
            app.UseStateAutoMapper();
        }
    }
}
