using CarBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarBackend.Controllers
{
    public class CartController : Controller
    {
        QLBanXeGanMayEntities data = new QLBanXeGanMayEntities();
        public ActionResult Index()
        {
            return View();
        }

        public List<Cart> GetCart()
        {
            List<Cart> listCart = Session["cart"] as List<Cart>;

            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["cart"] = listCart;
            }

            return listCart;
        }

        public ActionResult AddToCart(int carID, string strURL)
        {
            List<Cart> listCart = GetCart();

            Cart car = listCart.Find(b => b.carID == carID);

            if (car == null)
            {
                car = new Cart(carID);
                listCart.Add(car);
                return Redirect(strURL);
            }
            else
            {
                car.quantity++;
                return Redirect(strURL);
            }

        }

        private int TotalQuantity()
        {
            int total = 0;
            List<Cart> listCart = GetCart();
            if (listCart != null)
            {
                total = listCart.Sum(b => b.quantity);
            }
            return total;
        }

        private double TotalPrice()
        {
            double total = 0;
            List<Cart> listCart = GetCart();

            if (listCart != null)
            {
                total = listCart.Sum(c => c.total);
            }
            return total;
        }

        public ActionResult RemoveFromCart(int carID)
        {
            List<Cart> listCart = GetCart();

            Cart car = listCart.FirstOrDefault(b => b.carID == carID);

            if (car != null)
            {
                listCart.Remove(car);
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult UpdateCart(int carID, int quantity)
        {
            List<Cart> listCart = GetCart();

            Cart car = listCart.FirstOrDefault(b => b.carID == carID);

            if (car != null)
            {
                car.quantity = quantity;
            }

            return RedirectToAction("Cart");
        }

        public ActionResult ClearAll()
        {
            List<Cart> listCart = GetCart();
            listCart.Clear();

            return RedirectToAction("Cart");
        }

        public ActionResult Order()
        {
            if (Session["user"] == null || Session["user"].ToString() == "")
            {
                return RedirectToAction("Signin", "User");
            }

            List<Cart> listCart = GetCart();
            ViewBag.TotalPrice = TotalPrice();
            ViewBag.TotalQuantity = TotalQuantity();
            return View(listCart);
        }

        [HttpPost]
        public ActionResult Order(FormCollection collection)
        {
            DONDATHANG order = new DONDATHANG();
            KHACHHANG customer = (KHACHHANG)Session["user"];
            List<Cart> listCart = GetCart();
            order.MaKH = customer.MaKH;
            order.Ngaydat = DateTime.Now;
            order.Ngaygiao = DateTime.Now;
            order.Tinhtranggiaohang = false;
            order.Dathanhtoan = false;
            data.DONDATHANGs.Add(order);

            foreach (var item in listCart)
            {
                CHITIETDONTHANG orderDetail = new CHITIETDONTHANG();
                orderDetail.MaDonHang = order.MaDonHang;
                orderDetail.MaXe = item.carID;
                orderDetail.Soluong = item.quantity;
                orderDetail.Dongia = (decimal)item.price;
                data.CHITIETDONTHANGs.Add(orderDetail);
            }
            data.SaveChanges();
            Session["cart"] = null;
            return RedirectToAction("ConfirmOrder", "Cart");

        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }

        public ActionResult Cart()
        {
            List<Cart> listCart = GetCart();
            ViewBag.TotalPrice = TotalPrice();
            ViewBag.TotalQuantity = TotalQuantity();
            return View(listCart);
        }
    }
}