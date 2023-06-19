using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoProject.Models
{
    public class CommentModel 
    {
        [Key]
        public int Id { get; set; }
        public string Comments { get; set; }


        // relationship photo --> comment
        public int? PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public PhotoModel? Photo { get; set; }


        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public UserModel? Author { get; set; }
    }
}
