using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCFormData.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostType { get; set; }
        [DataType(DataType.DateTime)]
        public string PostDate { get; set; }
        [Required]
        public string PostImage { get; set; }
        [Required]
        public string PostContent { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}