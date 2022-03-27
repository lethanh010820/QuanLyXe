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
    public class AdminController : Controller
    {
        dbQLThuexeDataContext db = new dbQLThuexeDataContext();
        // GET: Admin
        public ActionResult Admin()
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
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
        public ActionResult QLKhachHang()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
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
                admin ad = db.admins.SingleOrDefault(n => n.taikhoan == tendn && n.matkhau == matkhau);
                if (ad != null)
                {
                    //ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Tkadmin"] = ad;
                    SetAlert("Đăng nhập thành công!!!", "success");
                    return RedirectToAction("Admin", "Admin");
                }
                else
                    SetAlert("Đăng nhập thất bại!!!", "error");
            }
            return View();
        }
        public ActionResult QLXe(int? page)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.maSP).ToPagedList(pageNumber, pageSize));

        }
        public ActionResult QLDonhang(int? page)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.CTHDs.ToList().OrderBy(n => n.maHD).ToPagedList(pageNumber, pageSize));

        }
        public ActionResult QLNguoidung(int? page)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.NGUOIDUNGs.ToList().OrderBy(n => n.maND).ToPagedList(pageNumber, pageSize));

        }
        //hien thi san pham
        public ActionResult Chitietxe(int id)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
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
        public ActionResult Themmoixe()
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
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
                    sp.image = fileName;
                    //luu vao db
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("QLXe");
            }
        }
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
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
            // bua sau lam thong bao
            return RedirectToAction("QLXe");
        }
        [HttpGet]
        public ActionResult Suaxe(int id)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
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
        [HttpPost, ActionName("Suaxe")]
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
            return RedirectToAction("QLXe");

        }
        public ActionResult Thongtinkhachhang(int id)
        {
            if (Session["Tkadmin"] == null || Session["Tkadmin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
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
            Session.Remove("Tkadmin");
            return RedirectToAction("Login");
        }
    }
}