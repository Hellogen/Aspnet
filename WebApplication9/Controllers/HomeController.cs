using Microsoft.AspNetCore.Mvc;
using WebApplication9.models;

namespace MvcApp.Controllers
{
    public class Home : Controller
    {
        
        
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return View();
            else
                return Redirect("/login");
            
        }
        [HttpGet]
        public async Task Add()
        {
            try
            {
                var context = HttpContext;
                
                string btnlike = Info.Horek.BTNlikeORdislike(context.User.Identity.Name);
                if (btnlike == "like")
                {
                    Info.Horek.AddLikes(context.User.Identity.Name);
                }
                else
                    Info.Horek.RemoveLikes(context.User.Identity.Name);
                Forsiteout outs = new Forsiteout(Info.Horek.ReturnLikes().ToString(), context.User.Identity.Name,Info. Horek.BTNlikeORdislike(context.User.Identity.Name), Info.Horek.ReturnComments());
                await Response.WriteAsJsonAsync(outs);

            }
            catch (Exception)
            {

            }
        }
        [HttpGet]
        public async Task Data()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var context = HttpContext;
                var posts = db.Post.Where(x => x.Banned == 0).ToArray();
                string[] datapostsname = new string[posts.Length];
                string[] datapostsID = new string[posts.Length];
                for (int i =0; i < posts.Length; i++)
                {
                    datapostsname[i] = posts[i].Name;
                    datapostsID[i] = posts[i].ID.ToString();
                }
                ReturnData data = new ReturnData(datapostsname, datapostsID);
                await context.Response.WriteAsJsonAsync(data);
            }
        }
        [HttpGet]
        public async Task Users()
        {
            var context = HttpContext;
            Forsiteout outs = new Forsiteout(Info.Horek.ReturnLikes().ToString(), context.User.Identity.Name, Info.Horek.BTNlikeORdislike(context.User.Identity.Name), Info.Horek.ReturnComments());
            await context.Response.WriteAsJsonAsync(outs);
        }
        [HttpPost]
        public async Task AddComment()
        {
            var context = HttpContext;
            var text = await context.Request.ReadFromJsonAsync<ForsiteIN>();
            string message;
            if (text != null)
            {
                message = text.ToString();
                Console.WriteLine(text.comment);
            }
            // если данные сконвертированы в Person
            Info.Horek.addComment(text.comment, context.User.Identity.Name);
        }
        [NonAction]
        public async Task TableOfLiked()
        {
            var context = HttpContext;
            string Usersname = context.User.Identity.Name;
            bool IsAdmin = true;
            if (IsAdmin)
            {
                string result = @"<!DOCTYPE html>
                 <html>
                 <head>
                   <meta charset='utf-8' />
                    <title>Admins Table</title>
                      </head>
                     <body>
       
                 ";

                List<string> Names = Info.Horek.ReturnLikesList();
                result += "<table border=\"1\">";
                for (int i = 0; i < Names.Count; i++)
                {
                    result += "<tr>";
                    result += "<td>" + i + "</td>" + "<td>" + Names[i] + "</td>";
                    result += "</tr>";
                }
                result += "</table>";
                result += " </ body ></ html > ";
                await Response.WriteAsync(result);
            }
            else
            {
                Response.StatusCode = 403;
                await Response.WriteAsync("403");
            }
        

        }
        record class Forsiteout(string likes, string name, string btn, string[] comments);
        public record class ForsiteIN(string comment);
        record class ReturnData(string[] PostName,string[] PostID);
    }
}