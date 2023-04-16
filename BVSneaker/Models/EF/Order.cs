using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    [Table("tb_Order")]
    public class Order : CommonAbstract
    {
        public Order()
        {
            IsDelivered = false;
            IsPaid = false;
            this.OrderDetail = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Order_Id { get; set; }
        public int? User_Id { get; set; }
        [Required]
        public bool IsDelivered { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Full name is required")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "E-mail is required")]
        public string Email { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }

    }
}