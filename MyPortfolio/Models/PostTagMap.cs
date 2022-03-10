namespace MyPortfolio.Models
{
    public interface IPostTagMap
    {
        PostModel PostModel { get; set; }
        int PostModel_ID { get; set; }
        TagModel TagModel { get; set; }
        int TagModel_ID { get; set; }
    }
}