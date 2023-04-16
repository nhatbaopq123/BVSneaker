using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_ProductImage")]
    public class ProductImage
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Image_Id { get; set; }
        public int Product_Id { get; set; }
        public string Image { get; set; }
        public bool isDefault { get; set; }

        public virtual Product Product { get; set; }
    }
}