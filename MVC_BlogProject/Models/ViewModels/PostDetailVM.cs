using BlogProject.Entities.Entities;

namespace MVC_BlogProject.Models.ViewModels
{
    public class PostDetailVM
    {
        public Post Post { get; set; }

        public Category Category { get; set; }

        public User User { get; set; }

        // TODO : İlgili postun comment listini de dahil edebiliriz.
        public List<Comment> Comments { get; set; }
    }
}
