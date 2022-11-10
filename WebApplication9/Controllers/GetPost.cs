using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApplication9.models;

namespace WebApplication9.Controllers
{
    public class GetPost : Controller
    {
        [HttpGet]
        async public void GetPosts(int id)
        {
            
              
                string post;
                using (FileStream stream = new FileStream(Info.pathToHomePost + id, FileMode.Open))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer,0,buffer.Length);
                    post = Encoding.UTF8.GetString(buffer);
                    await ControllerContext.HttpContext.Response.WriteAsync(post);
                }
            
            
        }
    }
}
