using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileProviders;
using WebApplication9.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // добавляем сервисы MVC
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/home/login");
builder.Services.AddAuthorization();
var app = builder.Build();

app.UseAuthentication();  
app.UseAuthorization();
app.UseStaticFiles();




app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    await context.Response.SendFileAsync("wwwroot/Login/Login.html");
});
app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
{
    using (DataBaseContext db = new DataBaseContext())
    {
       
        var user = db.Users.ToList();
        var form = context.Request.Form;
        string email = form["email"];
        string password = form["password"];
        bool equal = false;
        User? user1 = user.FirstOrDefault(x => x.Username == email && x.Password == password);
        if (user1 != null)
        {
            equal = true;
        }
        if (!equal)
        {
            return Results.Redirect("/login/Wrong/");
        }
        else
        {
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
    }
});
app.MapGet("/login/wrong/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    
   
    await context.Response.SendFileAsync("wwwroot/Login/LoginError.html");
});
app.MapPost("/login/wrong", async (string? returnUrl, HttpContext context) =>
{
    using (DataBaseContext db = new DataBaseContext())
    {

        var user = db.Users.ToList();
        var form = context.Request.Form;
        string email = form["email"];
        string password = form["password"];
        bool equal = false;

        User? user1 = user.FirstOrDefault(x => x.Username == email && x.Password == password);
        if (user1 != null)
        {
            equal = true;
        }
        if (!equal)
        {
            return Results.Redirect("/login/wrong");
        }
        else
        {
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
    }
});
app.MapGet("/register", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("wwwroot/Registration/Registration.html");
});
app.MapPost("/register", async (string? returnUrl, HttpContext context) =>
{
    using (DataBaseContext db = new DataBaseContext())
    {
        var user = db.Users.ToList();
        var form = context.Request.Form;
        string email = form["email"];
        string password = form["password"];
        bool equal = false;
        User? User1 = user.FirstOrDefault(x => x.Username == email);
        //User? user1 = user.Where(x => x.Username == email).FirstOrDefault();

        if (User1 == null)
        {
            var maxiduser = user.ToList();
            Console.WriteLine(maxiduser[maxiduser.Count() - 1].ID + 1);
            db.Users.Add(new User { ID = maxiduser[maxiduser.Count - 1].ID + 1, Admin = 0, Password = password, Username = email });
            await db.SaveChangesAsync();
            context.Response.Redirect("/");
        }
        else
        {
            context.Response.Redirect("/register/wrong");
        }
    }
});
app.MapGet("/register/wrong/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма для ввода логина/пароля
    await context.Response.SendFileAsync("wwwroot/Registration/RegistrationError.html");
});
app.MapPost("/register/wrong/", async (string? returnUrl, HttpContext context) =>
{
    using (DataBaseContext db = new DataBaseContext())
    {
        var user = db.Users.ToList();
        var form = context.Request.Form;
        string email = form["email"];
        string password = form["password"];
        bool equal = false;
        User? User1 = user.FirstOrDefault(x => x.Username == email);
        // User? user1 = user.Where(x => x.Username == email).FirstOrDefault();

        if (User1 == null)
        {
            var maxiduser = user.ToList();
            Console.WriteLine(maxiduser[maxiduser.Count() - 1].ID + 1);
            db.Users.Add(new User { ID = maxiduser[maxiduser.Count - 1].ID + 1, Admin = 0, Password = password, Username = email });
            await db.SaveChangesAsync();
            context.Response.Redirect("/");
        }
        else
        {
            context.Response.Redirect("/register/wrong");
        }
    }
});

app.MapGet("/ForgotAccount", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма для ввода логина/пароля
    await context.Response.SendFileAsync("wwwroot/ForgotAccount/forgotpassword.html");
});
app.MapPost("/ForgotAccount", async (string? returnUrl, HttpContext context) =>
{
    User? ForgotUser = null;
    var form = context.Request.Form;
    string email = form["email"];
    string password = form["password"];
    using (DataBaseContext db = new DataBaseContext())
    {
        var user = db.Users.ToList();
        User? user1 = user.FirstOrDefault(x => x.Username == email);
        if (user1 == null)
        {
            context.Response.Redirect("/ForgotAccount/wrong");
        }
        else
        {
            ForgotUser = user1;
        }
    }
    using (DataBaseContext db = new DataBaseContext())
    {
        if (ForgotUser != null)
        {
            ForgotUser.Password = password;
            db.Users.Update(ForgotUser);
            db.SaveChanges();
        }
    }
});
app.MapGet("/ForgotAccount/wrong", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма для ввода логина/пароля
    await context.Response.SendFileAsync("wwwroot/ForgotAccount/forgotpassworderror.html");
});
app.MapPost("/ForgotAccount/wrong", async (string? returnUrl, HttpContext context) =>
{
    User? ForgotUser = null;
    var form = context.Request.Form;
    string email = form["email"];
    string password = form["password"];
    using (DataBaseContext db = new DataBaseContext())
    {
        var user = db.Users.ToList();
        User? user1 = user.FirstOrDefault(x => x.Username == email);
        if (user1 == null)
        {
            context.Response.Redirect("/ForgotAccount/wrong");
        }
        else
        {
            ForgotUser = user1;
        }
    }
    using (DataBaseContext db = new DataBaseContext())
    {
        if (ForgotUser != null)
        {
            ForgotUser.Password = password;
            db.Users.Update(ForgotUser);
            db.SaveChanges();
        }
        context.Response.Redirect("/");
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

record class Person(string Email, string Password);
public record class Forsiteout(string likes, string name, string btn, string[] comments);
public record class ForsiteIN(string comment);