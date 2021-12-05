using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoManagerApp.DAL.Data;
using TodoManagerApp.DAL.Models;
using TodoManagerApp.BL;


namespace TodoManagerApi
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

            services.AddDbContext<DbTaskManagerContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DbTaskManagerContext"), b => b.MigrationsAssembly("TodoManagerApp.DAL"));
            });

            services.AddTransient<ColumnManager>();
            services.AddTransient<TodoManager>();

            services.AddTransient<IColumnRepository, ColumnRepository>();
            services.AddTransient<ITodoRepository, TodoRepository>();

            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy( 
                    builder => {
                        builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            services.AddRouting();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            

            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
