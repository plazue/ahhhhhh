using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BE1.Models;
using BE1.Models.viewmodel;
using System.Runtime.Remoting.Messaging;
using System.Web.Security;

namespace BE1.Areas.Customer.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities1 db = new MyStoreEntities1();
        // GET: Customer/Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }

                // Nếu chưa tồn tại thì tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password, // Lưu ý: Nên mã hóa mật khẩu trước khi lưu
                    UserRole = "Customer"
                };
                db.Users.Add(user);

                // Và tạo bản ghi thông tin khách hàng trong bảng Customer
                var customer = new BE1.Models.Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username
                };
                db.Customers.Add(customer);

                // Lưu thông tin tài khoản và thông tin khách hàng vào CSDL
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username
                                                      && u.Password == model.Password
                                                      && u.UserRole == "Customer");
                if (user != null)
                {
                    // Lưu trạng thái đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    // Lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }
        // GET: Customer/Account/ProfileInfo
        public ActionResult ProfileInfo()
        {
            string username = Session["Username"] as string;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var customer = db.Customers.SingleOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Account/ProfileInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileInfo(BE1.Models.Customer model, HttpPostedFileBase ImageFile)
        {
            string username = Session["Username"] as string;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var customer = db.Customers.SingleOrDefault(c => c.Username == username);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Cập nhật thông tin từ form
            customer.CustomerName = model.CustomerName;
            customer.CustomerPhone = model.CustomerPhone;
            customer.CustomerEmail = model.CustomerEmail;
            customer.CustomerAddress = model.CustomerAddress;

            // Xử lý upload ảnh đại diện mới nếu có chọn file
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string fileName = System.IO.Path.GetFileName(ImageFile.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/customers/"), fileName);
                ImageFile.SaveAs(path);
                customer.CustomerImage = "/Images/customers/" + fileName;
            }

            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return View(customer);
        }
    }
}