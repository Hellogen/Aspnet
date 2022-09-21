using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Text;


namespace WebApplication9.Controllers
{
    public class Content : Controller
    {
       // string path = @"Database\Posts\";   // путь к файлу

        string text = "Hello METANIT.COM"; // строка для записи
        public void Index(int ID)
        {
           var context = ControllerContext;
            if (ID is not 0)
            {
                string post = "";
                 //context.HttpContext.Response.WriteAsync(ID.ToString());
                FileStream? fstream = null;
                try
                {
                    fstream = new FileStream(Info.pathtopost + ID, FileMode.Open);
                    byte[] buffer = new byte[fstream.Length];
                    fstream.Read(buffer,0, buffer.Length);
                    post = Encoding.UTF8.GetString(buffer);
                    //Console.WriteLine(post);
                   
                }
                catch (Exception ex)
                { 
                    context.HttpContext.Response.WriteAsync("error 404");
                }
                finally
                {
                    fstream?.Close();
                }
                    
                    context.HttpContext.Response.WriteAsync((string)post);

            }
            else
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
        int i = 0;
        [NonAction]
        public async void act()
        {
            await Task.Run(() =>
            {
                while (i < 3)
                {
                    ControllerContext.HttpContext.Response.WriteAsync(i.ToString());
                    Task.Delay(1).Wait();
                    i++;
                    ControllerContext.ValueProviderFactories.Clear();
                }

            
            });
            
        }

        
    }

}
