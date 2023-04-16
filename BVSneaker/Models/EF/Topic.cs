using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_Topic")]
    public class Topic : CommonAbstract
    {
        public Topic()
        {
            this.Product = new HashSet<Product>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Topic_Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 kí tự")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 kí tự")]
        public string DetailTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Alias { get; set; }
        public ICollection<Product> Product { get; set; }

    }
}