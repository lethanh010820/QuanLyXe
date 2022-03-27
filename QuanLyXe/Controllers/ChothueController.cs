using PagedList;
using QuanLyXe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyXe.Controllers
{
    public class ChothueController : Controller
    {
        // GET: Chothue

        dbQLThuexeDataContext db = new dbQLThuexeDataContext();
        public ActionResult Chothuexe()
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        public ActionResult QLHangXe(int? page)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.LOAISANPHAMs.ToList().OrderBy(n => n.maloaiSP).ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult Themmoihang()
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoihang(LOAISANPHAM lsp)
        {
            db.LOAISANPHAMs.InsertOnSubmit(lsp);
            db.SubmitChanges();
            return RedirectToAction("QLHangXe");
           
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {

            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên tài khoản";

            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";

            }
            else
            {
                // Gán giá trị cho đối tượng đc tạo mới
                NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(n => n.tentaikhoan == tendn && n.matkhau == matkhau);
                if (nd != null)
                {
                    //ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["TKND"] = nd;
                    SetAlert("Đăng nhập thành công!!!", "success");
                    return RedirectToAction("Chothuexe", "Chothue");
                }
                else
                    SetAlert("Tài khoản hoặc mật khẩu không đúng!!!", "error");
            }
            return View();
        }
        public ActionResult QLChothue()
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];

            var sp = db.SANPHAMs.OrderByDescending(a => a.maND).Where(a => a.maND == nd.maND).ToList();

            return View(sp);
        }
        [HttpGet]
        public ActionResult Themmoixe()
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];
            //Dua du lieu vào dropdownlist
            //lay ds tu table chu de va sap sep theo ten chu de, chon lay gia tri ma cd, hien thi tenchude
            ViewBag.maLoaiSP = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.tenloaiSP), "maloaiSP", "tenloaiSP");
            ViewBag.makv = new SelectList(db.KHUVUCs.ToList().OrderBy(n => n.tenkv), "makv", "tenkv");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoixe(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];
            //đưa dữ liệu vào dropdownload
            ViewBag.maLoaiSP = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.tenloaiSP), "maloaiSP", "tenloaiSP");
            ViewBag.makv = new SelectList(db.KHUVUCs.ToList().OrderBy(n => n.tenkv), "makv", "tenkv");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "vui long chon anh bia";
                return View();
            }
            // them vao db
            else
            {
                if (ModelState.IsValid)
                {
                    // luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    //kiemtra hinh anh da ton tai chua
                    if (System.IO.File.Exists(path))

                        ViewBag.Thongbao = "hình ảnh đã tồn tại";

                    else
                    {
                        //lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    sp.maND = nd.maND;
                    sp.image = fileName;
                    //luu vao db
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                    SetAlert("Thêm thành công!!!", "success");
                }
                return RedirectToAction("QLChothue");
            }
        }
        public ActionResult Chitietxe(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // lay sach theo ma
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.maSP == id);
            ViewBag.MaSP = sp.maSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Suaxe(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            //lay sach theo ma id
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.maSP == id);
            ViewBag.maSP = sp.maSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.maLoaiSP = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(n => n.tenloaiSP), "maloaiSP", "tenloaiSP");
            ViewBag.makv = new SelectList(db.KHUVUCs.ToList().OrderBy(n => n.tenkv), "makv", "tenkv");
            return View(sp);
        }
        [HttpPost, ValidateInput(false), ActionName("Suaxe")]
        public ActionResult XacnhanSuaxe(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.maSP == id);
            ViewBag.maSP = sp.maSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(sp);
            db.SubmitChanges();
            SetAlert("Sửa thành công!!!", "success");
            return RedirectToAction("QLChothue");

        }
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // lay sach theo ma id
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.maSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            SetAlert("Xóa thành công!!!", "success");
            return RedirectToAction("QLChothue");
        }
        public ActionResult HoaDon(int? page)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];

            var sp = db.CTHDs.OrderByDescending(a => a.isSell).Where(a => a.isSell == nd.maND).ToPagedList(pageNumber, pageSize);
            return View(sp);

        }
        public ActionResult XoaHoaDon(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // lay sach theo ma id
            CTHD hd = db.CTHDs.SingleOrDefault(n => n.maHD == id);
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CTHDs.DeleteOnSubmit(hd);
            db.SubmitChanges();
            SetAlert("Xóa thành công!!!", "success");
            return RedirectToAction("HoaDon");
        }
        [HttpGet]
        public ActionResult SuaHoadon(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            ViewBag.tinhtrangthue = new SelectList(db.TIENDOTHUEXEs.ToList().OrderBy(n => n.maTD), "maTD", "tinhtrangthuexe");
            //lay sach theo ma id
            HOADON sp = db.HOADONs.SingleOrDefault(n => n.maHD == id);
            ViewBag.maHD = sp.maHD;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("SuaHoadon")]
        public ActionResult XnSuaHoadon(int id)
        {
            HOADON sp = db.HOADONs.SingleOrDefault(n => n.maHD == id);
            ViewBag.maHD = sp.maHD;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(sp);
            SetAlert("Cập nhật thành công!!!", "success");
            db.SubmitChanges();
            return RedirectToAction("Hoadon");

        }
        public ActionResult QLBlog()
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];

            var sp = db.BLOGDULICHes.OrderByDescending(a => a.maND).Where(a => a.maND == nd.maND).ToList();

            return View(sp);
        }
        [HttpGet]
        public ActionResult ThemmoiBlog()
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiBlog(BLOGDULICH bl, HttpPostedFileBase fileUpload)
        {
            NGUOIDUNG nd = (NGUOIDUNG)Session["TKND"];
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!!";
                return View();
            }
            // them vao db
            else
            {
                if (ModelState.IsValid)
                {
                    // luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    //kiemtra hinh anh da ton tai chua
                    if (System.IO.File.Exists(path))

                        ViewBag.Thongbao = "Hình ảnh đã tồn tại!!";

                    else
                    {
                        //lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    bl.maND = nd.maND;
                    bl.ngaydang = DateTime.Now;
                    bl.hinhanh = fileName;
                    bl.luotxem = 1;
                    //luu vao db
                    db.BLOGDULICHes.InsertOnSubmit(bl);
                    db.SubmitChanges();
                    SetAlert("Thêm thành công!!!", "success");
                }
                return RedirectToAction("QLBlog");
            }
        }
        public ActionResult ChitietBlog(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // lay sach theo ma
            BLOGDULICH bl = db.BLOGDULICHes.SingleOrDefault(n => n.maTD == id);
            ViewBag.maTD = bl.maTD;
            if (bl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bl);
        }
        public ActionResult XoaBlog(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // lay sach theo ma id
            BLOGDULICH bl = db.BLOGDULICHes.SingleOrDefault(n => n.maTD == id);
            if (bl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.BLOGDULICHes.DeleteOnSubmit(bl);
            db.SubmitChanges();
            SetAlert("Xóa thành công!!!", "success");
            return RedirectToAction("QLBlog");
        }
        [HttpGet]
        public ActionResult Suablog(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // lay sach theo ma id
            BLOGDULICH bl = db.BLOGDULICHes.SingleOrDefault(n => n.maTD == id);
            if (bl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bl); ;
        }
        [HttpPost, ValidateInput(false), ActionName("Suablog")]
        public ActionResult Xacnhansuablog(int id, HttpPostedFileBase fileUpload)
        {
            // lay sach theo ma id
            BLOGDULICH bl = db.BLOGDULICHes.SingleOrDefault(n => n.maTD == id);
            if (bl == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(bl);
            db.SubmitChanges();
            SetAlert("Sửa thành công!!!", "success");
            return RedirectToAction("QLBlog");
        }
        public ActionResult Thongtinkhachhang(int id)
        {
            if (Session["TKND"] == null || Session["TKND"].ToString() == "")
            {
                return RedirectToAction("Login", "Chothue");
            }
            // l
            // lay sach theo ma
            NGUOIDUNG nd = db.NGUOIDUNGs.SingleOrDefault(n => n.maND == id);
            ViewBag.maND = nd.maND;
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nd);
        }
        public ActionResult Dangxuat()
        {
            Session.Remove("TKND");
            return RedirectToAction("Login");
        }
    }
}