using PagedList;
using QuanLyXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyXe.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLThuexeDataContext data = new dbQLThuexeDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult Dangky(FormCollection collection, NGUOIDUNG kh)
        {
            
            String tentaikhoan = collection["Tentaikhoan"];
            String matkhau = collection["Matkhau"];
            String matkhau2 = collection["Matkhau2"];
            String hoten = collection["hoten"];
            String sodienthoai = collection["sodienthoai"];
            String email = collection["email"];

            if (String.IsNullOrEmpty(tentaikhoan))
            {
                ViewData["Loi1"] = " Nhập tên đăng nhập";
                SetAlert("Thông tin đăng ký không hợp lệ!!!", "error");

            }

            if (String.IsNullOrEmpty(matkhau))
            {
                SetAlert("Thông tin đăng ký không hợp lệ!!!", "error");
                ViewData["Loi2"] = " Phải nhập mật khẩu";

            }
           if (String.IsNullOrEmpty(matkhau2))
            {
                SetAlert("Thông tin đăng ký không hợp lệ!!!", "error");
                ViewData["Loi3"] = " Phải nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(hoten))
            {
                SetAlert("Thông tin đăng ký không hợp lệ!!!", "error");
                ViewData["Loi4"] = " Họ và Tên không được để trống";

            }
            if (String.IsNullOrEmpty(sodienthoai))
            {
                SetAlert("Thông tin đăng ký không hợp lệ!!!", "error");
                ViewData["Loi5"] = " Phải nhập số điện thoại";

            }
            if (String.IsNullOrEmpty(email))
            {
                SetAlert("Thông tin đăng ký không hợp lệ!!!", "error");
                ViewData["Loi6"] = " Email không được bỏ trống";

            }
           
            else
            {
                if(matkhau.Equals(matkhau2))
                {
                    //Gán giá trị cho đối tượng được tạo mới

                    kh.tentaikhoan = tentaikhoan;
                    kh.matkhau = matkhau;
                    kh.hoten = hoten;
                    kh.sodienthoai = sodienthoai;
                    kh.email = email;

                    data.NGUOIDUNGs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    SetAlert("Đăng ký thành công!!!", "success");
                    return RedirectToAction("Dangnhap");
                }
                else
                {
                    SetAlert("Mật khẩu không trùng khớp!!!", "error");
                }
               
            }
            return this.Dangky();
        }
        [HttpGet]

        public ActionResult Dangnhap()
        {
            return View();
        }

        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu:";

            }
            else
            {
                //Gán giá trị đối tượng đc tạo mới
                NGUOIDUNG kh = data.NGUOIDUNGs.SingleOrDefault(n => n.tentaikhoan == tendn && n.matkhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    SetAlert("Đăng nhập thành công!!!", "success");
                    return Redirect("/");
                }
                else
                {
                    SetAlert("Thông tin đăng nhập không hợp lệ!!!", "error");

                }
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

        public ActionResult Dangxuat()
        {
            Session.Remove("Taikhoan");
            return Redirect("/");
        }
        public ActionResult Thongtincanhan(int id)
        {
            // lay sach theo ma
            NGUOIDUNG nd = data.NGUOIDUNGs.SingleOrDefault(n => n.maND == id);
            ViewBag.MaND = nd.maND;
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nd);
        }
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            //lay sach theo ma id
            NGUOIDUNG nd = data.NGUOIDUNGs.SingleOrDefault(n => n.maND == id);
            ViewBag.maND = nd.maND;
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nd);
        }
        [HttpPost, ActionName("Suathongtin")]
        public ActionResult Xacnhansua(int id)
        {
            NGUOIDUNG nd = data.NGUOIDUNGs.SingleOrDefault(n => n.maND == id);
            ViewBag.maND = nd.maND;
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(nd);
            data.SubmitChanges();
            return Redirect("/");

        }
        public ActionResult HoaDon(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            NGUOIDUNG nd = (NGUOIDUNG)Session["Taikhoan"];

            var sp = data.CTHDs.OrderByDescending(a => a.maHD).Where(a => a.HOADON.maND == nd.maND).ToPagedList(pageNumber, pageSize);
            return View(sp);

        }
    }
 }
