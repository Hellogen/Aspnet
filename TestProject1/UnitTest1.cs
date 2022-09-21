using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WebApplication9.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace TestProject1
{
    [TestClass]
    public class PostTest
    {
        [TestMethod]
        public void addlikes()
        {
            string testname = "Hello";
            int expectlikes = 1;
            POST Test = new POST(1);
            Test.AddLikes(testname);
            Assert.AreEqual(expectlikes, Test.ReturnLikes(), "не добавило лайки корректно");

        }
        [TestMethod]
        public void returnlistcorrectrly()
        {
            POST NewTest = new POST(1);

            List<string> people = new List<string>();
            people.Add("vasya");
            people.Add("sanya");
            people.Add("kek");
            for (int i = 0; i < people.Count; i++)
            {
                NewTest.AddLikes(people[i]);

            }
            for (int i = 0; i < people.Count; i++)
                Assert.AreEqual(people[i], NewTest.ReturnLikesList()[i], "Не добавило людей корректно");
            Assert.AreEqual(people.Count, NewTest.ReturnLikesList().Count, "неадекватное количество человек (пустые места)");


        }
        [TestMethod]
        public void TestBtnLikes()
        {
            POST NewTest = new POST(1);

            List<string> people = new List<string>();
            people.Add("vasya");
            NewTest.AddLikes(people[0]);
            string btn = NewTest.BTNlikeORdislike(people[0]);
            Assert.AreEqual(btn, "dislike", "сломалось переключение лайка");
        }
        [TestMethod]
        public async void TestDatabase()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
           



            }
        } 
        

     }
    
   
    
}
