using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BE1.Models;
using BE1.Models.ViewModel;

namespace BE1.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities1 db = new MyStoreEntities1();

        // GET: Admin/Products
        // GET: Admin/Products
        public ActionResult Index(string searchTerm, decimal? minPrice, decimal? maxPrice)
        {
            var model = new SearchProductVM();

            // Giữ lại các giá trị tìm kiếm để hiển thị lại trên form
            model.SearchTerm = searchTerm;

            var products = db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                                               p.ProductDecription.Contains(searchTerm) ||
                                               p.Category.CategoryName.Contains(searchTerm));
            }

            // 2. Lọc theo giá tối thiểu
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }

            // 3. Lọc theo giá tối đa
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }


            model.Products = products.ToList();


            return View(model);
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh
                if (product.ImageFile != null && product.ImageFile.ContentLength > 0)
                {
                    // Lấy tên file
                    string fileName = System.IO.Path.GetFileName(product.ImageFile.FileName);
                    // Tạo đường dẫn lưu file trên server
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/products/"), fileName);

                    // Lưu file
                    product.ImageFile.SaveAs(path);

                    // Lưu đường dẫn vào database
                    product.ProductImage = "/Images/products/" + fileName;
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm hiện tại trong cơ sở dữ liệu ra để so sánh/giữ lại ảnh cũ nếu không upload ảnh mới
                var existingProduct = db.Products.AsNoTracking().FirstOrDefault(p => p.ProductID == product.ProductID);

                if (product.ImageFile != null && product.ImageFile.ContentLength > 0)
                {
                    // Xử lý upload ảnh mới
                    string fileName = System.IO.Path.GetFileName(product.ImageFile.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/products/"), fileName);
                    product.ImageFile.SaveAs(path);

                    // Gán đường dẫn ảnh mới
                    product.ProductImage = "/Images/products/" + fileName;
                }
                else
                {
                    // Nếu không chọn ảnh mới, giữ nguyên đường dẫn ảnh cũ từ DB
                    if (existingProduct != null)
                    {
                        product.ProductImage = existingProduct.ProductImage;
                    }
                }

                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }
        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
