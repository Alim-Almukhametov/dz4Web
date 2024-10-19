
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using WebAppFinal.Abstraction;
using WebAppFinal.Controllers;
using WebAppFinal.DB;
using WebAppFinal.RSATools;

namespace WebAppFinal
{
    public class Program
    {
        /*
         openssl genpkey -algorithm RSA -out private_key.pem
         openssl rsa -pubout -in private_key.pem -out public_key.pem

        Первая команда генерирует приватный ключ и сохраняет его в каталог под именем
        private_key.pem
        Вторая команда получает публичный ключ на основе информации, содержащейся в
        файла private_key.pem, и сохраняет ее в файл public_key.pem
         */
        /*
        "Jwt": {
                "Key": "MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDMn8JdH9+7G0Dx",
                "Issuer": "https://localhost:7250/",
                "Audience": "http://localhost:7250/"
            }
        */

        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UserContext>();


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserAutoficationService, UserAutoficationService>();
            /*
             Метод создаёт объект класса RSA на основе информации
            прочитанной из файла с публичным ключом(ImportFromPem)
             
            Сейчас метод возвращает объект RSA на основе файла с 
            публичным ключём, но может возвращать RSA на основе приватного
             
             */
            static RSA GetPublicKey()
            {
                var readKeyFile = File.ReadAllText("RSATools/public_key.pem");
                var rsa = RSA.Create();
                rsa.ImportFromPem(readKeyFile);
                return rsa;

            }


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    // явное указание использования RSA-ключа
                    //IssuerSigningKey = new RsaSecurityKey(GetPublicKey())
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                }
            );

            //builder.Services.AddAuthentication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //Порядок вызова имеет значение
            app.UseAuthentication();
            app.UseAuthorization();
            


            app.MapControllers();

            app.Run();
        }
    }
}
