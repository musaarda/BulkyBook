using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            })
        };


        if (id == null || id == 0)
        {
            // Create Product
            // ViewBag.CategoryList = CategoryList;
            // ViewData["CoverTypeList"] = CoverTypeList;

            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            // Update the product
            return View(productVM);
        }
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images/products");
                var extension = Path.GetExtension(file.FileName);

                if (obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                obj.Product.ImageUrl = @"/images/products/" + fileName + extension;
            }

            if (obj.Product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.Product);
            }
            else
            {
                _unitOfWork.Product.Update(obj.Product);
            }

            _unitOfWork.Save();
            TempData["Success"] = "Product created successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var objFromDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
        if (objFromDb == null)
            return NotFound();

        return View(objFromDb);
    }

    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
            return NotFound();

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["Success"] = "Product deleted successfully.";
        return RedirectToAction("Index");
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult DeleteApi(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
            return Json(new { success = false, message = "Error while deleting." });

        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successfull." });
    }
    #endregion

}
