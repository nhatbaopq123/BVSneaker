using BVSneaker.Models;
using BVSneaker.Models.EF;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BVSneaker.Models.ShoppingCart;
using System.Security.Principal;
using System.Configuration;
using BVSneaker.Models.Payments;

namespace BVSneaker.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext _dbConnect = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart!=null)
            {
                return View(cart.Items);
            }
            return View();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult Partial_Item_CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();

        }

            [HttpPost]
        public ActionResult Update(int id, int quantity, int stock)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            int currentQuantity = cart.GetCurrentQuantity(id);
            if (cart != null)
            {
                var checkProduct = _dbConnect.Product.FirstOrDefault(x => x.Product_Id == id);
                var productStock = _dbConnect.ProductStock.FirstOrDefault(x => x.Size == stock && checkProduct.Product_Id == x.Product_Id);
                if (productStock.Quantity < quantity)
                {
                    return Json(new { Success = false, msg = "This product is out of stock at this moment" });
                }
                else
                {
                    cart.UpdateQuanity(id, quantity);
                    if (quantity < currentQuantity)
                    {
                        productStock.Quantity = productStock.Quantity + (currentQuantity - quantity);
                        currentQuantity = quantity;
                    }
                    else
                    {
                        productStock.Quantity = productStock.Quantity - (quantity - currentQuantity);
                        currentQuantity = quantity;
                    }
                    return Json(new { Success = true });
                }
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = -1, count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult AddtoCart(int id, int quantity, decimal price, int stock)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };
            var checkProduct = _dbConnect.Product.FirstOrDefault(x => x.Product_Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                var productStock = _dbConnect.ProductStock.FirstOrDefault(x => x.Size == stock && checkProduct.Product_Id == x.Product_Id);
                if (productStock.Quantity < quantity)
                {
                    code = new { Success = false, msg = "This product is out of stock at this moment", code = 1, count = cart.Items.Count };
                }
                else
                {
                    ShoppingCartItem items = new ShoppingCartItem
                    {
                        ProductId = checkProduct.Product_Id,
                        ProductName = checkProduct.Name,
                        ProductAlias = checkProduct.Alias,
                        CategoryName = checkProduct.Category.Name,
                        TopicName = checkProduct.Topic.Title,
                        Quantity = quantity,
                    };
                    _dbConnect.ProductStock.Attach(productStock);
                    productStock.Quantity = productStock.Quantity - quantity;
                    if (checkProduct.ProductImage.FirstOrDefault(x => x.isDefault) != null)
                    {
                        items.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.isDefault).Image;
                    }
                    items.Stock = stock;
                    items.Price = price;
                    items.TotalPrice = items.Quantity * items.Price;
                    cart.AddToCart(items, quantity);
                    Session["Cart"] = cart;
                    code = new { Success = true, msg = "Item added successfully", code = 1, count = cart.Items.Count };
                }
            }
            return Json(code);
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.CheckCart = cart;
            }
            var user = new OrderViewModel()
            {
                CustomerName = "",
                Phone = "",
                Email = "",
            };
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUserId = User.Identity.GetUserId();
                var currentUser = _dbConnect.Users.FirstOrDefault(x => x.Id == currentUserId);
                user.CustomerName = currentUser.FullName;
                user.Phone = currentUser.Phone;
                user.Email = currentUser.Email;
            }
            return View(user);
        } 
     

        public ActionResult CheckOutSuccess()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, code = -1, Url="" };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    cart.Items.ForEach(x => order.OrderDetail.Add(new OrderDetail
                    {
                        Product_Id = x.ProductId,
                        Quanity = x.Quantity,
                        Price = x.Price,
                        Order_Id = order.Order_Id
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    if (order.TotalAmount < 50)
                        order.TotalAmount += 5.00m;
                    order.PaymentMethod = req.PaymentMethod;
                    order.Email = req.Email;
                    order.OrderDate = DateTime.Now;
                    order.CreateBy = req.Phone;
                    order.ModBy = req.Phone;
                    order.CreateDate = DateTime.Now;
                    order.ModDate = DateTime.Now;
                    for (int i = 0; i < cart.Items.Count; i++)
                    {
                        var item = cart.Items[i];
                        var checkProduct = _dbConnect.Product.FirstOrDefault(x => x.Product_Id == item.ProductId);
                        var productStock = _dbConnect.ProductStock.FirstOrDefault(x => x.Size == item.Stock && checkProduct.Product_Id == x.Product_Id);
                        productStock.Quantity = productStock.Quantity - item.Quantity;
                    }
                    _dbConnect.Order.Add(order);
                    _dbConnect.SaveChanges();
                    //send mail cho customer sau khi đặt hàng
                    //Có thể hủy đơn hàng trước kkhi ad min duyệt đơn hàng
                    //cần gửi yêu cầu xác nhận chờ duyệt sau đó mới gửi chấp nhận đơn hàng
                    var strProduct = "";
                    var total = decimal.Zero;
                    var totalAmount = decimal.Zero;
                    foreach (var pd in cart.Items)
                    {
                        strProduct += "<tr>";
                        strProduct += "<td>" + pd.ProductName + pd.Stock + " " + "</td>";
                        strProduct += "<td>" + pd.Quantity + "</td>";
                        strProduct += "<td>" + "<span>$</span>" + pd.TotalPrice.ToString("0.00") + "</td>";
                        strProduct += "</tr>";
                        total += pd.Price * pd.Quantity;

                    }
                    if (total < 50)
                        totalAmount = total + 17.99m;
                    else
                        totalAmount = total;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{OrderID}}", order.Order_Id.ToString());
                    contentCustomer = contentCustomer.Replace("{{Product}}", strProduct);
                    contentCustomer = contentCustomer.Replace("{{CustomerName}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", order.Email);
                    contentCustomer = contentCustomer.Replace("{{Address}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{PlaceOrderDate}}", order.ModDate.ToString());
                    contentCustomer = contentCustomer.Replace("{{Total}}", total.ToString("0.00"));
                    contentCustomer = contentCustomer.Replace("{{TotalAmount}}", totalAmount.ToString("0.00"));
                    BVSneaker.Common.Common.SendMail("BVSneaker", "Order #" + order.Order_Id.ToString(), contentCustomer.ToString(), req.Email);
                    cart.ClearCart();
                    //var url = UrlPayment(req.TypePaymentVN);
                    //code = new { Success = true, code = -1 };
                    return RedirectToAction("CheckOutSuccess");

                }
            }
            return Json(req);
        }


        //Thanh toán VNPay
        public string UrlPayment(int TypePaymentVN, int orderId)
        {
            var urlPayment = "";
            var order = _dbConnect.Order.FirstOrDefault(x => x.Order_Id == orderId);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.TotalAmount * 24000).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreateDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "$");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng:" + order.Order_Id);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Order_Id.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }

    }
        
}
