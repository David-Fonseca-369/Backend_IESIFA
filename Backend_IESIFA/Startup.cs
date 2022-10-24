using Backend_IESIFA.Filters;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {

            ////Servicio AutoMapper            
            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"), builder =>
                //{
                //    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                //});

                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"));

            });            

            //Agregar cabcera en cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });

            //Apply global filter
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            }
            );


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {           
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //Permite servir archivos estaticos
            //app.UseStaticFiles();

            app.UseRouting();
            
            app.UseCors();




            //Para autorizarte, primero tienes que autenticarte
            app.UseAuthentication();


            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}