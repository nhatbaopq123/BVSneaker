using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_Size")]
    public class Size : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Size_Id { get; set; }

        [Required(ErrorMessage = "Không được để trống size giày")]
        public double SizeValue { get; set; }

        public List<ProductPrice> ProductPrice { get; set; }
    }
}