using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_Brand")]
    public class Brand
    {
        public Brand()
        {
            this.Product = new HashSet<Product>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Brand_Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 kí tự")]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Alias { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}