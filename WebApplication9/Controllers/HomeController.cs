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

       
        record class Forsiteout(string likes, string name, string btn, string[] comments);
        public record class ForsiteIN(string comment);
        record class ReturnData(string[] PostName,string[] PostID);
    }
}