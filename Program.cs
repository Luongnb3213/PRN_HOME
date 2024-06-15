using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN221_Assignment.Respository;
using System.Security.Claims;
using System.Text;

namespace PRN221_Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<DataContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DB") ?? throw new InvalidOperationException("Connection string 'DataContext' not found.")));
            var jwtOptions = builder.Configuration.GetSection("JwtOptions");
            var key = Encoding.ASCII.GetBytes(jwtOptions["SigningKey"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>

{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwtToken"];
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            if (!context.Response.HasStarted)
            {
                context.Response.Redirect("/authentication/login");
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions["Issuer"],
        ValidAudience = jwtOptions["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
}).AddCookie().AddGoogle(options =>
{
    options.ClientId = "500915612685-fmucrlmjuti8p7q1jop9vbu0bk48sevi.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

}); 
            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}