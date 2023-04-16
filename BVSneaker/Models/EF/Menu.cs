using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_Menu")]
    public class Menu : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Menu_Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 kí tự")]
        public string Title { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Position { get; set; }
        public string Alias { get; set; }
    }
}