using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCFormData.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required(ErrorMessage = "This field cannot be empty")]
        public string CommentContent { get; set; }
        [DataType(DataType.DateTime)]
        public string CommentDate { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}