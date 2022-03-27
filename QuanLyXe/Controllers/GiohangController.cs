using QuanLyXe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyXe.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }
        dbQLThuexeDataContext data = new dbQLThuexeDataContext();
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
        //Lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang == null)
            {
                //Neu gio hang chua ton tai thif tao listGiohang
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;

            }
            return listGiohang;
        }

        //Them gio hang
        public ActionResult Themgiohang(int imaSP, string strURL)
        {
            //:ay ra Session gio hang
            List<Giohang> listGiohang = Laygiohang();
            //Ktra sach nay ton tai trong session chua
            Giohang sp = listGiohang.Find(n => n.imaSP == imaSP);
            if (sp == null)
            {
                sp = new Giohang(imaSP);
                listGiohang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoluong++;
                return Redirect(strURL);
            }
        }
        public ActionResult Themgiohang1(int imaSP, string strURL)
        {
            //:ay ra Session gio hang
            List<Giohang> listGiohang = Laygiohang();
            //Ktra sach nay ton tai trong session chua
            Giohang sp = listGiohang.Find(n => n.imaSP == imaSP);
            if (sp == null)
            {
                sp = new Giohang(imaSP);
                listGiohang.Add(sp);
            }
            else
            {
                sp.iSoluong++;
            }
            return RedirectToAction("Giohang");
        }
        //Tinh tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                iTongSoLuong = listGiohang.Sum(n => n.iSoluong);

            }
            return iTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double dTongTien = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                dTongTien = listGiohang.Sum(n => n.dThanhtien);
            }
            return dTongTien;
        }
        //Xay dung trang Giohang
        public ActionResult Giohang()
        {
            List<Giohang> listGiohang = Laygiohang();

            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(listGiohang);
        }
        public ActionResult Giohangpartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult Voucher(string voucher)
        {
            List<Giohang> listGiohang = Laygiohang();
            if (voucher == "ABCDEFG")
            {
                int voucherFree = 20000;
                ViewBag.Tongtien = TongTien() - voucherFree;
                ViewBag.Voucher = " Mã Voucher giảm: 20.000 VNĐ";

            }
            else
            {
                ViewBag.Tongtien = TongTien();
                ViewBag.Voucher = "Mã Voucher không đúng";
            }
            Session["tongtien"] = ViewBag.Tongtien;
            ViewBag.Tongsoluong = TongSoLuong();
            return View(listGiohang);
        }
        public ActionResult XoaGiohang(int imaSP)
        {
            //Lay gio hang tu session
            List<Giohang> listGiohang = Laygiohang();
            //ktra sach da co trong session gio hang
            Giohang sp = listGiohang.SingleOrDefault(n => n.imaSP == imaSP);
            //Neu san pham ton tai thi co sua lai soluong
            if (sp != null)
            {
                listGiohang.RemoveAll(n => n.imaSP == imaSP);
                return RedirectToAction("Giohang");
            }
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("index", "ThueXe");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Capnhatgiohang(int imaSP, FormCollection f)
        {
            //lay gio hang tu session
            List<Giohang> listGiohang = Laygiohang();
            //ktra sach co trong session gio hang
            Giohang sp = listGiohang.SingleOrDefault(n => n.imaSP == imaSP);
            //Neu ton tai thi cho sua so luong
            if (sp != null)
            {
                sp.iSoluong = int.Parse(f["inputSL"].ToString());
                if (sp.iSoluong == 0)
                {
                    listGiohang.RemoveAll(n => n.imaSP == imaSP);
                }
            }
            SetAlert("Cập nhật thành công!!!", "success");
            return RedirectToAction("Giohang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiemtra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("index", "Moto");
            }
            //Lay gio hang tu session
            List<Giohang> listGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(listGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            //Them Don hang
            HOADON ddh = new HOADON();
            NGUOIDUNG kh = (NGUOIDUNG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.maND = kh.maND;
            ddh.ngaythue = DateTime.Now;
            var ngaytra = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.ngaytra = DateTime.Parse(ngaytra);
            ddh.tinhtrangthue = 1;
            data.HOADONs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //them chi tiet don hang
            foreach (var item in gh)
            {
                CTHD ctdh = new CTHD();
                ctdh.maHD = ddh.maHD;
                ctdh.maSP = item.imaSP;
                ctdh.isSell = item.imaND;
                ctdh.soluong = item.iSoluong;
                ctdh.thanhtien = (decimal)item.dThanhtien;
                data.CTHDs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            SetAlert("Vui lòng đợi phản hồi từ phía người cho thuê!!!", "warning");
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}