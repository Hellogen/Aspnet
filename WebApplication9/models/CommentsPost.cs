
namespace WebApplication9.models
{
    public class CommentsPost
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long PostID { get; set; }
        public string Comment { get; set; } = null!;
    }
}

