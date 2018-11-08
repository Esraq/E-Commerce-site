using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCFormData.Models
{
    public class User
    {
        public int UserId { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required(ErrorMessage = " This field cannot be empty")]
        public string UserName { get; set; }
        [Required(ErrorMessage = " This field cannot be empty")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 3)]
        public string UserPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = " Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
        public string UserType { get; set; }
        [Required(ErrorMessage = " This field cannot be empty")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserEmail { get; set; }
        public string UserProfilePicture { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
    }
}