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

        // Şağ tarafta görünecek kategori listesi

        public List<Category> Categories { get; set; }


        // Sağ tarafta RelatedPost Listesi

        public List<Post> RelatedPost { get; set; } = new List<Post>(3);
    }
}
