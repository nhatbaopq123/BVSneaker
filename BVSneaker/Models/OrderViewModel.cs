using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BVSneaker.Models
{
    public class OrderViewModel
    {
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
    }
}