public static class Info
{
   // public static List<User> newpeople = new List<User>
   // { new User(1,"Hello",true)};

    public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=models\\DataBases\\Workers.mdb;";
    public static string pathtoDatabase = @"Database\";
    public static string pathtopost = pathtoDatabase + @"Posts\";
/*
    public static int UsersCheckAdmin(string Username)
    {
        for (int i = 0; i < newpeople.Count; i++)
        {
            if (newpeople[i].Username == Username)
            {
                return newpeople[i].Admin == 1 ?  1 : 0;
            }
        }   
        return 0;
    }
*/
}

