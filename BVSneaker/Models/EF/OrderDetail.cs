using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderDetail_Id { get; set; }
        public int Order_Id { get; set; }
        public int Product_Id { get; set; }
        public int Quanity { get; set; }
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}