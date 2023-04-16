using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BVSneaker.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
        }

        [HttpPost]
        public void AddToCart(ShoppingCartItem item, int Quanity)
        {
            var checkExist = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExist != null)
            {
                checkExist.Quantity += Quanity;
                checkExist.TotalPrice = checkExist.Price * checkExist.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExist = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                Items.Remove(checkExist);
            }
        }
        public void UpdateQuanity(int id, int quantity)
        {
            var checkExist = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                checkExist.Quantity = quantity;
                checkExist.TotalPrice = checkExist.Price * checkExist.Quantity;
            }
        }
        public int GetCurrentQuantity(int id)
        {
            var checkExist = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                return checkExist.Quantity;
            }
            return 0;
        }
        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);
        }
        public int GetTotalQuanity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }
    }
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductAlias { get; set; }
        public string ProductImg { get; set; }
        public string TopicName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal TotalPrice { get; set; }
    }
}