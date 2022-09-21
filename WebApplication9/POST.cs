
public class POST
{
    private int id;
    private List<string> likesList = new List<string>();
    List<string> Comments = new List<string> {};
        private int likes = 0;
        public string path;
        public POST(int ID)
        {
            id = ID;
        }
        public void AddLikes(string Name)
        {
            likesList.Add(Name);
        
            likes++;
            for (int i = 0; i < likesList.Count; i++)
            {
                Console.WriteLine(likesList[i]);
            }
            Console.WriteLine("-");
        }
        public void RemoveLikes(string Name)
        {
            bool win = likesList.Remove(Name);
            if (win)
                likes--;
            else
                Console.WriteLine("no");
        }
        public int ReturnLikes()
        {
            return likes;
        }
        public List<string> ReturnLikesList()
        {
            return likesList;
        }
        public int FindLikes(string Name)
        {
            int findedid = likesList.IndexOf(Name);
            return findedid;
        }
        public string BTNlikeORdislike(string Name) // решает в зависимости от пользователя будет ли видна кнопка или нет
        {
  
            string btn;
           
            if (FindLikes(Name) == -1)
            {
            btn = "like";
            }
            else
            {
            btn = "dislike";
            }
            return btn;
        }
        public void addComment(string comment, string name)
        {
        Comments.Add(name);
        Comments.Add(comment);
        }
        public string[] ReturnComments()
        {
            int count = Comments.Count;
            string[] newmes = new string[count];

            for (int i =0; i < newmes.Length; i++)
            {
            
                newmes[i] = Comments[i];
            
            }
            return newmes;
        }
        
    }

