using PagedList;
using QuanLyXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyXe.Controllers
{
    public class BlogController : Controller
    {
        dbQLThuexeDataContext db = new dbQLThuexeDataContext();
        public ActionResult TrangBlog(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.BLOGDULICHes.ToList().OrderBy(n => n.maTD).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TrangChitietBlog(int id)
        {
            BLOGDULICH bl = db.BLOGDULICHes.SingleOrDefault(n => n.maTD == id);
            ViewBag.maTD = bl.maTD;
            if (bl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                bl.luotxem++;
                UpdateModel(bl);
                db.SubmitChanges();
            }
            return View(bl);
        }
        public ActionResult Top3blogmoi()
        {
            var item = db.BLOGDULICHes.ToList().OrderBy(n => n.maTD).Take(3);
            return PartialView(item);
        }
        public ActionResult TimkiemBlog(string searchString)
        {
            var bl = from s in db.BLOGDULICHes select s;
            if (!String.IsNullOrEmpty(searchString))

            {
                bl = bl.Where(s => s.tieude.Contains(searchString) || s.mota.Contains(searchString));
            }
            
            return View(bl);
        }
    }
}