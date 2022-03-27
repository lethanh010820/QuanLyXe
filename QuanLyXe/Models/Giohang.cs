using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyXe.Models
{
    public class Giohang
    {
        dbQLThuexeDataContext data = new dbQLThuexeDataContext();

        public int imaSP { set; get; }
        public string stenSP { set; get; }
        public string simage { set; get; }
        public string smausac { set; get; }
        public string sbienso { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public int imaND { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int maSP)
        {
            imaSP = maSP;
            SANPHAM sanpham = data.SANPHAMs.Single(n => n.maSP == imaSP);
            stenSP = sanpham.tenSP;
            simage = sanpham.image;
            smausac = sanpham.mausac;
            sbienso = sanpham.bienso;
            imaND = (int)sanpham.maND;
            dDongia =  double.Parse(sanpham.giathue.ToString());
            iSoluong = 1;
        }
        public Giohang()
        {
        }
    }
}