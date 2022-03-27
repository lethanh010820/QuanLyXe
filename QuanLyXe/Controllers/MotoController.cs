using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyXe.Controllers
{
    public class MotoController : Controller
    {
        // GET: Moto
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chinhsachbaomat()
        {
            return View();
        }
        public ActionResult cuahang()
        {
            return View();
        }
        public ActionResult FAQs()
        {
            return View();
        }
    }
}