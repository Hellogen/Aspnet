using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication9.models;
namespace WebApplication9.Controllers
{
    public class requests : Controller
    {
        //
        [HttpPost]
        public async Task Add()
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext())
                {
                    string[] comments = new string[0];
                    Console.WriteLine("add");
                    var context = HttpContext;
                    var text = await context.Request.ReadFromJsonAsync<ReturnID>();
                    string btn = "";
                    
                    // Console.WriteLine(text.ID);

                    try
                    {
                        var user = db.Users.Where(x => x.Username == context.User.Identity.Name).FirstOrDefault();
                        var likeslist = db.LikesList.Where(x => x.PostID == long.Parse(text.ID) && x.UserID == user.ID);
                        var likesofid = db.LikesList.Where(x => x.PostID == long.Parse(text.ID)).ToList();
                        var likesListtable = db.LikesList.ToList();
                        if (likeslist.Any())
                        {
                            btn = "like";
                            db.LikesList.Remove(likeslist.FirstOrDefault());
                            db.SaveChanges();
                        }
                        else
                        {
                            btn = "dislike";
                            if (likesListtable.Any())
                            { 
                                 LikesList like = new LikesList() { ID = likesListtable[likesListtable.Count()-1].ID + 1, PostID = long.Parse(text.ID), UserID = user.ID, Like = 0 };
                                db.LikesList.Add(like);
                                db.SaveChanges();
                            }
                            else
                            {
                                LikesList like = new LikesList() { ID = 1, PostID = long.Parse(text.ID), UserID = user.ID, Like = 0 };
                                db.LikesList.Add(like);
                                db.SaveChanges();
                            }
                            
                            
                        }
                    }
                    catch (Exception ex)
                    { Console.WriteLine("error: " +ex.Message); }

                    var countlikes = db.LikesList.Where(x => x.PostID == long.Parse(text.ID)).Count();
                   // countlikes = db.LikesList.Count();
                    Forsiteout outs = new Forsiteout(countlikes.ToString(), context.User.Identity.Name, btn, comments);
                    await Response.WriteAsJsonAsync(outs);
                }

            }
            catch (Exception)
            {

            }
        }
        
        [HttpPost]
        public async Task Users()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
 
                var context = HttpContext;
                var text = await context.Request.ReadFromJsonAsync<ReturnID>();
                var comments = db.CommentsPost.Where(x => x.PostID == long.Parse(text.ID)).ToList();
                var users = db.Users.ToList();
                string btn;


                var likelist = db.LikesList.Where(x => x.PostID == long.Parse(text.ID)).ToList();
                var user = db.Users.Where(x => x.Username == context.User.Identity.Name).FirstOrDefault();
                var likelistID = db.LikesList.Where(x => x.UserID == user.ID && x.PostID == long.Parse(text.ID)).ToList();
                if (likelistID.Any())
                {
                    btn = "dislike";
                }
                else
                {
                    btn = "like";
                }    






                 
                /// 1 строка - имя
                /// 2 строка - коммент
                int count = comments.Count;
                string[] newcom = new string[count*2];
                int comcount = 0;
                for (int i = 0; i < newcom.Length; )
                {
                    var userwhocomment = users.Where(x => x.ID == comments[comcount].UserID).ToList();
                    newcom[i] = userwhocomment[0].Username;
                    newcom[i + 1] = comments[comcount].Comment;
                    i = i + 2;
                    comcount++;
                }
                Forsiteout outs = new Forsiteout(likelist.Count().ToString(), context.User.Identity.Name, btn, newcom);
                await context.Response.WriteAsJsonAsync(outs);
            }
        }
        [HttpPost]
        public async Task AddComment()
        {
            using (DataBaseContext db = new DataBaseContext())
            {

                var context = HttpContext;
                var text = await context.Request.ReadFromJsonAsync<ADDComment>();
                string message;
                if (text != null)
                {
                    message = text.ToString();
                    Console.WriteLine(text.ID + " /" + text.comment);
                }
                if (text.comment != "" || context.User.Identity.Name != "" || context.User.Identity.Name != null)
                {
                    var user = db.Users.Where(x => x.Username == context.User.Identity.Name).ToList();
                    CommentsPost newcomment = new CommentsPost() { ID = db.CommentsPost.Count() + 1, UserID = user[0].ID, PostID = long.Parse(text.ID), Comment = text.comment };
                    db.CommentsPost.Add(newcomment);
                    db.SaveChanges();
                }
               // Info.Horek.addComment(text.comment, context.User.Identity.Name);
            }
        }
        record class ReturnID(string ID);
        record class ADDComment(string ID, string comment);
        record class Forsiteout(string likes, string name, string btn, string[] comments);
    }
}
