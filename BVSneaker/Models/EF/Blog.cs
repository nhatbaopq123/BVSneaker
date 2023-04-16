using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Models.EF
{
    [Table("tb_Blog")]
    public class Blog : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Blog_Id { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "This field is limited to 150 characters")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [AllowHtml]
        [Required]
        public string Content { get; set; }
        [Required]
        public string CoverImage { get; set; }
        [Required]
        public string Author { get; set; }
    }
}