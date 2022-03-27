namespace QuanLyXe.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        public long id { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [StringLength(100)]
        [Display(Name = "email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(1000)]
        [Display(Name = "Mật khẩu")]
        public string matKhau { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống!")]
        [StringLength(100)]
        [Display(Name = "Tên người dùng")]
        public string tenCH { get; set; }

        [StringLength(1000)]
        [Display(Name = "Địa chỉ")]
        public string diaChi { get; set; }

        [StringLength(11)]
        [Display(Name = "Số điện thoại")]
        public string sodienthoai { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? ngayTao { get; set; }

        [Display(Name = "Ngày cập nhập")]
        public DateTime? ngayCapNhat { get; set; }

        
    }
}
