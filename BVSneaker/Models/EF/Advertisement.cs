using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_Advertisement")]
    public class Advertisement : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Adv_Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 kí tự")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }
        public string Alias { get; set; }
    }
}