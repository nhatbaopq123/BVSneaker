using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BVSneaker.Models.EF
{
    [Table("tb_Product")]
    public class Product : CommonAbstract
    {
        public Product()
        {
            this.ProductImage = new HashSet<ProductImage>();
            this.OrderDetail = new HashSet<OrderDetail>();
            this.ProductStock = new List<ProductStock>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Product_Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 kí tự")]
        public string Name { get; set; }
        public float Rate { get; set; }
        [AllowHtml]
        [Required]
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public bool isHome { get; set; }
        public bool isBestSeller { get; set; }
        public bool isFeature { get; set; }
        public bool isHot { get; set; }
        public int Brand_Id { get; set; }
        public int Category_Id { get; set; }
        public int Topic_Id { get; set; }
        public string Alias { get; set; }


        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public ICollection<ProductStock> ProductStock { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual Topic Topic { get; set; }

    }
}