using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TransportManage_WebApiCore.Data;

namespace TransportManage_WebApiCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
          
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(op =>
            {
                op.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                op.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //zop.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                op.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                op.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Provide Token",
                    Name = "Authorization",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
          {
            new OpenApiSecurityScheme()
            {
              Reference = new OpenApiReference()
              {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
              }
            },
             Array.Empty<string>()
          }
        });
            });

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAngularApp",
            //        builder =>
            //        {
            //            builder.WithOrigins("http://localhost:4200")
            //                   .AllowAnyHeader()
            //                   .AllowAnyMethod()
            //                   .AllowCredentials();
            //        });
            //});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            builder.Services.AddDbContext<TransportDb>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Media"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>()
              .AddEntityFrameworkStores<TransportDb>()
              ;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme
                = options.DefaultScheme
                = options.DefaultChallengeScheme
                = options.DefaultForbidScheme
                = options.DefaultSignInScheme
                = options.DefaultSignOutScheme
                = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(bearer =>
            {

                byte[] key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"));
                //byte[] key = Encoding.UTF8.GetBytes(Key);

                bearer.RequireHttpsMetadata = false;


                bearer.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)

                };

            });
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Editor", policyOpt =>
                {
                    //policyOpt.RequireUserName("admin");
                    //policyOpt.RequireRole("Admin");
                    //policyOpt.RequireRole("Moderator");

                    policyOpt.RequireAssertion(ctx => ctx.User.Claims.Any(c => (c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Moderator")) || (c.Type == ClaimTypes.Name && c.Value.ToLower().Contains("admin"))));

                });

            });

            builder.Services.AddScoped<IImageUpload, ImageUpload>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

     //       app.UseStaticFiles(new StaticFileOptions
     //       {
     //           FileProvider = new PhysicalFileProvider(
     //Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
     //           RequestPath = "/Uploads"
     //       });

            app.UseRouting();
            app.UseAuthentication();
            // app.UseCors("AllowAngularApp");
            app.UseCors("AllowReactApp");

            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
