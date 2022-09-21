using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace WebApplication9.models
{

    public partial class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Post> Post { get; set; } = null!;
        public virtual DbSet<LikesList> LikesList { get; set; } = null!;
        public virtual DbSet<CommentsPost> CommentsPost { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlite("Data Source=D:" + Info.pathtoDatabase + "\\DataBase.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }




}
