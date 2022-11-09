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
            if (id == 1)
            {
              
                string post;
                using (FileStream stream = new FileStream(Info.pathtopost + id, FileMode.Open))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer,0,buffer.Length);
                    post = Encoding.UTF8.GetString(buffer);
                    await ControllerContext.HttpContext.Response.WriteAsync(post);
                }
            }
            else
            {
                await ControllerContext.HttpContext.Response.WriteAsJsonAsync("none");
            }
            
        }
    }
}
