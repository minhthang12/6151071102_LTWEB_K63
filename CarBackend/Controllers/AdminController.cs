using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarBackend.Controllers
{
    public class AdminController : Controller
    {
        QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();


        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Cars(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;

            var cars = db.XEGANMAYs.OrderBy(n => n.MaXe).ToPagedList(pageNumber, pageSize);

            return View(cars);
        }


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var username = collection["username"];
            var password = collection["password"];


            if (String.IsNullOrEmpty(username))
            {
                ViewData["err1"] = "Please input username";
                return View();
            }

            else if (String.IsNullOrEmpty(password))
            {
                ViewData["err2"] = "Please input password";
                return View();
            }

            else
            {
                Admin admin = db.Admins.FirstOrDefault(ctm => ctm.UserAdmin == username && ctm.PassAdmin == password);

                if (admin != null)
                {
                    ViewBag.announcement = "Success to sign in";
                    Session["Taikhoanadmin"] = admin;
                }
                else
                {
                    ViewBag.announcement = "Fail to sign in";
                    return View();
                }

            }

            return RedirectToAction("Index");
        }

        public ActionResult AddNewCar()
        {
            var model = new XEGANMAY();
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewCar(XEGANMAY car, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");

            if (fileUpload == null)
            {
                ViewBag.Announce = "Please choose a picture";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), filename);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Announce = "Picture is existed";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    car.Anhbia = filename;

                    db.XEGANMAYs.Add(car);
                    db.SaveChanges();

                }
                return RedirectToAction("Cars", "Admin");
            }

        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Details(int id)
        {
           var car = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == id);
            ViewBag.MaXe = car?.MaXe;
            if(car == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(car);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var car = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == id);
            ViewBag.MaXe = car?.MaXe;
            if (car == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var car = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == id);
            ViewBag.MaXe = car?.MaXe;
            if (car == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.XEGANMAYs.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Cars", "Admin");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var car = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == id);

            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe", car.MaLX);
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", car.MaNPP);
            if (car == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(car);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(XEGANMAY car, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe", car.MaLX);
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", car.MaNPP);

            if (ModelState.IsValid)
            {
                var existingCar = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == car.MaXe);
                if (existingCar == null)
                {
                    ViewBag.Announce = "Car not found.";
                    return View(car);
                }

                existingCar.TenXe = car.TenXe;
                existingCar.Giaban = car.Giaban;
                existingCar.Mota = car.Mota;
                existingCar.Ngaycapnhat = car.Ngaycapnhat;
                existingCar.Soluongton = car.Soluongton;
                existingCar.MaLX = car.MaLX;
                existingCar.MaNPP = car.MaNPP;

                if (fileUpload != null)
                {
                    var filename = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), filename);

                    if (!System.IO.File.Exists(path))
                    {
                        fileUpload.SaveAs(path);
                        existingCar.Anhbia = filename;
                    }
                    else
                    {
                        ViewBag.Announce = "Picture already exists.";
                        return View(car);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Cars", "Admin");
            }

            ViewBag.Announce = "Model state is invalid.";
            return View(car);
        }

        [HttpGet]
        public ActionResult CarStatistics()
        {
            var cars = db.XEGANMAYs
                .GroupBy(c => c.LOAIXE)
                .ToList();

            return View(cars);
        }


    }
}