using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarBackend.Controllers
{
    public class UserController : Controller
    {
        QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(FormCollection collection, KHACHHANG cus)
        {
            var customerName = collection["customerName"];
            var username = collection["username"];
            var password = collection["password"];
            var confirmPassword = collection["confirmPassword"];
            var address = collection["address"];
            var email = collection["email"];
            var phone = collection["phone"];
            var birthday = String.Format("{0:MM/dd/yyyy}", collection["birthday"]);

            if (String.IsNullOrEmpty(customerName))
                ViewData["err1"] = "Please input your name";
            else if (String.IsNullOrEmpty(username))
                ViewData["err2"] = "Please input username";
            else if (String.IsNullOrEmpty(password))
                ViewData["err3"] = "Please input password";
            else if (password != confirmPassword)
                ViewData["err4"] = "Please confirm password";
            else if (String.IsNullOrEmpty(email))
                ViewData["err5"] = "Please input email";
            else if (String.IsNullOrEmpty(phone))
                ViewData["err6"] = "Please input phone";
            else
            {
                cus.HoTen = customerName;
                cus.Taikhoan = username;
                cus.Matkhau = password;
                cus.DiachiKH = address;
                cus.Email = email;
                cus.DienthoaiKH = phone;
                cus.Ngaysinh = DateTime.Parse(birthday);
                db.KHACHHANGs.Add(cus);
                db.SaveChanges();
            }

            return RedirectToAction("Signin");
        }

        [HttpGet]
        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(FormCollection collection)
        {

            var username = collection["username"];
            var password = collection["password"];


            if (String.IsNullOrEmpty(username))
                ViewData["err1"] = "Please input username";
            else if (String.IsNullOrEmpty(password))
                ViewData["err2"] = "Please input password";

            else
            {
                KHACHHANG customer = db.KHACHHANGs.FirstOrDefault(ctm => ctm.Taikhoan == username && ctm.Matkhau == password);

                if (customer != null)
                {
                    ViewBag.announcement = "Success to sign in";
                    Session["user"] = customer;
                }
                else
                {
                    ViewBag.announcement = "Fail to sign in";
                }

            }

            return RedirectToAction("Index", "CarStore");
        }
    }
}