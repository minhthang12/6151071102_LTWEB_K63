using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarBackend.Controllers
{
    public class CarStoreController : Controller
    {
        QLBanXeGanMayEntities data = new QLBanXeGanMayEntities();

        private List<XEGANMAY> GetCar(int count)
        {
            return data.XEGANMAYs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var car = GetCar(15);
            return View(car.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Type()
        { 
            var type = from cd in data.LOAIXEs select cd;
            return View(type); 
        }

        public ActionResult Distributor()
        {
            var distributor = from d in data.NHAPHANPHOIs select d;
            return View(distributor);
        }

        public ActionResult CarWithType(int id)
        {
            var car = from c in data.XEGANMAYs where c.MaLX == id select c;
            return View(car);
        }

        public ActionResult CarWithDistributor(int id)
        {
            var car = from c in data.XEGANMAYs where c.MaNPP == id select c;
            return View(car);
        }

        public ActionResult Details(int id)
        {
            var car = from c in data.XEGANMAYs
                      where c.MaXe == id select c;
            return View(car.Single());
        }

    }
}