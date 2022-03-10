using System.Data.Entity;

namespace MyPortfolio.Models
{
    public class BlogContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BlogContext() : base("name=BlogContext")
        {
        }

        public DbSet<CatagoryModel> CatagoryModels { get; set; }

        public DbSet<CommentModel> CommentModels { get; set; }

        public DbSet<PostModel> PostModels { get; set; }

        public DbSet<TagModel> TagModels { get; set; }

        //public IDbSet<TagModelPostModel> TagModelPostModels { get; set; }

        public DbSet<ContactModel> ContactModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostModel>()
                .HasMany(t => t.Tags)
                .WithMany(p => p.PostModels)
                .Map(pt =>
                {
                    pt.MapLeftKey("PostRefId");
                    pt.MapRightKey("TagRefId");
                    pt.ToTable("PostTagMap");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
