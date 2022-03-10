using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPortfolio.Models
{

    #region BlogStructure
    public class BlogCatagory
    {
        public CatagoryModel Catagory { get; set; }
        public IEnumerable<CatagoryModel> Catagories { get; set; }
    }

    public class BlogTags
    {
        public TagModel Tag { get; set; }
        public IEnumerable<TagModel> Tags { get; set; }
    }
    #endregion

    #region BlogModel
    public class PostModel
    {
        public PostModel()
        {
            this.Tags = new HashSet<TagModel>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }

        //If Post modified by user 
        public bool IsModified { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        //Main Post Content Holder
        [Required]
        [AllowHtml]
        [Display(Name = "Post")]
        [DataType(DataType.MultilineText)]
        public string BodyContent { get; set; }

        //Meta Content
        //[Required]
        [Display(Name = "Summary")]
        public string ShortContent { get; set; }
        [Display(Name = "Meta Url")]
        public string UrlSlug { get; set; }

        //Post Catagory Relation
        [Display(Name = "Catagory")]
        public int CatagoryID { get; set; }
        public virtual CatagoryModel Catagory { get; set; }

        //User Info
        //[Required]
        public string UserID { get; set; }
        //public ApplicationUser User { get; set; }

        //Tag List
        //public TagModelPostModel TagID { get; set; }
        public virtual ICollection<TagModel> Tags { get; set; }

        //Comment List
        public virtual List<CommentModel> Comments { get; set; }

        public string tag;
        public string TagString
        {
            get { return tag; }
            set { tag = value; }
        }
        
    }

    public class CommentModel
    {
        [Key]
        public int ID { get; set; }
        //User Info
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "User Email")]
        public string UserEmail { get; set; }
        //User Comment
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        public string Content { get; set; }
        //Comment Date
        [Display(Name = "Comment Date")]
        public DateTime Date { get; set; }

        //If User is not a guest
        public bool IsUserHaveId { get; set; }
        public string UserID { get; set; }
        //public ApplicationUser User { get; set; }

        //Related Post
        //[Required]
        public int PostID { get; set; }
        public virtual PostModel Post { get; set; }
    }

    public class CatagoryModel
    {
        //Post Catagory
        [Key]
        public int ID { get; set; }
        [Required]
        public string Catagory { get; set; }

        //public string UrlSlug { get; set; }
        //public string Description { get; set; }

        //Related Posts
        public virtual List<PostModel> Posts { get; set; }
    }

    public class TagModel
    {
        //Post Tag
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Url Slug")]
        public string Url { get; set; }

        public string Description { get; set; }

        //Related Posts
        public virtual IList<PostModel> PostModels { get; set; }
    }

    #endregion

    #region Others

    #endregion
}