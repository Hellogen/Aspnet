using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using WebApplication9.models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // добавляем сервисы MVC
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/home/login");
builder.Services.AddAuthorization();
var app = builder.Build();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();
app.UseStaticFiles();
//POST Horek = new POST(1);
// условная бд с пользователями


// устанавливаем сопоставление маршрутов с контроллерами
app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма для ввода логина/пароля
    string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>input name please</label><br />
                <input name='email' />
            </p>

            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
{
    using (DataBaseContext db = new DataBaseContext())
    {
        var user = db.Users.ToList();
        var form = context.Request.Form;
        string email = form["email"];
        bool equal = false;
        
        for (int i = 0; i < user.Count; i++)
        {
            if (user[i].Username == email)
            {
                equal = true;
            }
        }
        if (!equal)
        {
            db.Users.Add(new User { ID = user.Count + 1, Username = email, Password = "123", Admin = 0 });
            db.SaveChanges();
            user = db.Users.ToList();
            for (int i = 0; i < user.Count; i++) //
            {
                Console.WriteLine(user[i].ID + ":" + user[i].Username);
            }
            Console.WriteLine(user.Count);
        }
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };
        // создаем объект ClaimsIdentity
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        // установка аутентификационных куки
        AuthenticationProperties properties = new AuthenticationProperties();
        properties.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3);
        properties.IsPersistent = true;
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
        var us = from User in db.Users
                 where User.Admin == 0
                 select User;
        foreach (var use in us)
            Console.WriteLine(use.Username);
        return Results.Redirect(returnUrl ?? "/");
    }
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(name: "Content", pattern: "{controller=Content}/{action}/{ID}");
app.MapControllerRoute(name: "Requests", pattern: "{controller=Requests}/{action}");
app.Run();


public record class Forsiteout(string likes, string name, string btn, string[] comments);
public record class ForsiteIN(string comment);