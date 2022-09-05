using Microsoft.AspNetCore.Mvc.Formatters;
using AdventureWorksNS.Data;
using static System.Console;
using AdventureWorksAPI.Repositories;

namespace AdvetureWorksAPI
{
    public class program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Agregar el contexto de la base de datos de Adventure Works        
            builder.Services.AdventureWorksDBContext();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                WriteLine("Formatters por omision:");
                foreach (IOutputFormatter formatter in options.OutputFormatters)
                {
                    OutputFormatter? mediaFormatter = formatter as OutputFormatter;
                    if (mediaFormatter == null)
                    {
                        WriteLine($" {formatter.GetType().Name}");
                    }
                    else
                    {
                        
                        WriteLine($" {mediaFormatter.GetType().Name}," +
                            $"Media Types: {string.Join(",", mediaFormatter.SupportedMediaTypes)}");
                    }
                }
            })

                .AddXmlDataContractSerializerFormatters()
                .AddXmlSerializerFormatters(); 
            
            
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            //builder.Services.AddScoped<ICustumerRepository, CustomerRepository>(); //esquemas de repositorio para que salga en el swager 
            //var app = builder.Build();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IProductCategory, ProductCategoryRepository>(); //esquemas de repositorio para que salga en el swager 
            var app = builder.Build();






            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }


}