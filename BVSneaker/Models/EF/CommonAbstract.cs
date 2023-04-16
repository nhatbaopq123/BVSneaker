using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVSneaker.Models.EF
{
    public class CommonAbstract
    {
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModDate { get; set; }
        public string ModBy { get; set; }
    }
}