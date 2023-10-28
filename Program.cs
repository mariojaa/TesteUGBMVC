using Microsoft.EntityFrameworkCore;
using TesteUGB.Data;
using TesteUGB.Repositorio;

namespace TesteUGBMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicione a leitura das configurações do appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Services.AddDbContext<TesteUGBDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
            });
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
            builder.Services.AddScoped<ProdutoRepository>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<UsuarioRepository>();
            builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            builder.Services.AddScoped<FornecedorRepository>();
            builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
            builder.Services.AddScoped<ServicoRepository>();
            builder.Services.AddMvc();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}