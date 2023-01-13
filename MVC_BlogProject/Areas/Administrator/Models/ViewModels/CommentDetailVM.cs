using BlogProject.Entities.Entities;

namespace MVC_BlogProject.Areas.Administrator.Models.ViewModels
{
    public class CommentDetailVM
    {
        public Comment Comment { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
