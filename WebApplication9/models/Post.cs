using Microsoft.EntityFrameworkCore;

namespace WebApplication9.models
{
    public class Post
    {
        public long ID { get; set; }
        public string? Name { get; set; }

        public int Banned { get; set; }
    }


}
