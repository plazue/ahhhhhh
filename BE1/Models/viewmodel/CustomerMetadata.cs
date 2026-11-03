using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace BE1.Models
{
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
        // Thuộc tính tạm để nhận file ảnh upload từ form (không lưu trực tiếp vào cột DB dưới dạng object)
        [DataType(DataType.Upload)]
        [Display(Name = "Ảnh đại diện")]
        public HttpPostedFileBase ImageFile { get; set; }
    }

    public class CustomerMetadata
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Họ tên khách hàng không được để trống.")]
        [StringLength(255, ErrorMessage = "Họ tên không được vượt quá 255 ký tự.")]
        [Display(Name = "Họ & Tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ (phải từ 10-11 chữ số).")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Địa chỉ email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "Tên tài khoản (Username) không được để trống.")]
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string CustomerImage { get; set; }

        [Display(Name = "Trạng thái hoạt động")]
        public Nullable<bool> IsActive { get; set; }

        [Display(Name = "Quyền")]
        public string Role { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}