using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_ProductStock")]
    public class ProductStock
    {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int Stock_Id { get; set; }
            public int Product_Id { get; set; }
            [Required(ErrorMessage = "Please fill quantity field")]
            public int Quantity { get; set; }
            [Required(ErrorMessage = "Please fill size field")]
            public int Size { get; set; }
            [Required(ErrorMessage = "Please fill price field")]
            public decimal Price { get; set; }

            public virtual Product Product { get; set; }
        
    }
}
