using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyXe.Models;
using PagedList;

namespace QuanLyXe.Controllers
{
    public class ThueXeController : Controller
    {
        dbQLThuexeDataContext data = new dbQLThuexeDataContext();
        // GET: ThueXe
        public ActionResult Index(int? page, String searchString)
        {
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 6;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            var bl = from s in data.SANPHAMs select s;
            if (!String.IsNullOrEmpty(searchString))

            {
                bl = bl.Where(s => s.tenSP.Contains(searchString));
            }
            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(bl.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TheLoai()
        {
            var theloai = from tl in data.LOAISANPHAMs select tl;
            return PartialView(theloai);
        }
        public ActionResult KhuVuc()
        {
            var KV = from kv in data.KHUVUCs select kv;
            return PartialView(KV);
        }
        public ActionResult SPTheoTheLoai(int id)
        {
            var moto = from s in data.SANPHAMs where s.maloaiSP == id select s;
            return PartialView(moto);
        }
        public ActionResult SPTheoKhuvuc(int id)
        {
            var moto = from s in data.SANPHAMs where s.makv == id select s;
            return PartialView(moto);
        }
        public ActionResult Top5spmoi()
        {
            var moto = from s in data.SANPHAMs select s;
            return PartialView(moto);
        }
        public ActionResult ChiTiet(int id)
        {
            var sp = from s in data.SANPHAMs
                       where s.maSP == id
                       select s;
            return View(sp.Single());
        }
        public ActionResult Timkiem(string searchString)
        {
            var sp = from s in data.SANPHAMs select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sp = sp.Where(s => s.tenSP.Contains(searchString));
            }
            return View(sp);
        }
        public ActionResult XeTayGa ()
        {
            var sp = from s in data.SANPHAMs
                     where s.tenSP == "Xe AB"
                     select s;
            return View(sp);
        }
        public ActionResult XeCaoCao()
        {
            var sp = from s in data.SANPHAMs
                     where s.tenSP == "Xe Cào Cào"
                     select s;
            return View(sp);
        }
        public ActionResult XeEx()
        {
            var sp = from s in data.SANPHAMs
                     where s.tenSP == "Xe EX"
                     select s;
            return View(sp);
        }
        public ActionResult XeWareRSX()
        {
            var sp = from s in data.SANPHAMs
                     where s.tenSP == "Xe Ware RSX"
                     select s;
            return View(sp);
        }
        public ActionResult XeWare()
        {
            var sp = from s in data.SANPHAMs
                     where s.tenSP == "Xe Ware"
                     select s;
            return View(sp);
        }
    }
}