using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;
using WebApplication9.models;
using static System.Net.Mime.MediaTypeNames;

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
        [HttpGet] // возвращает список сайтов на меню выбора сайтов
        public async Task Data()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                string nicedayuser = "";
                var context = HttpContext;
                var posts = db.Post.Where(x => x.Banned == 0).ToArray();
                string[] datapostsname = new string[posts.Length];
                string[] datapostsID = new string[posts.Length];
                for (int i =0; i < posts.Length; i++)
                {
                    datapostsname[i] = posts[i].Name;
                    datapostsID[i] = posts[i].ID.ToString();
                }
                try
                {
                    using (FileStream stream = new FileStream(Info.pathtoNiceDay, FileMode.Open, FileAccess.Read))
                    {
                        Random rnd = new Random();
                        byte[] buffer = new byte[stream.Length];
                        await stream.ReadAsync(buffer, 0, buffer.Length);
                        string textniceday = Encoding.UTF8.GetString(buffer);
                        string[] massiveniceday = textniceday.Split('\n');
                        int value = rnd.Next(0, massiveniceday.Length);
                        nicedayuser = massiveniceday[value];
                        nicedayuser = nicedayuser.Replace("%username%", context.User.Identity.Name);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                ReturnData data = new ReturnData(datapostsname, datapostsID, nicedayuser);
                await context.Response.WriteAsJsonAsync(data);
            }
        }

       
        record class Forsiteout(string likes, string name, string btn, string[] comments);
        public record class ForsiteIN(string comment);
        record class ReturnData(string[] PostName,string[] PostID, string nicedayusername);
    }
}