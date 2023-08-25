using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();

        return View(objCategoryList);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Name.");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category created successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
        if (categoryFromDb == null)
            return NotFound();

        return View(categoryFromDb);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Name.");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
        if (categoryFromDb == null)
            return NotFound();

        return View(categoryFromDb);
    }

    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
            return NotFound();

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["Success"] = "Category deleted successfully.";
        return RedirectToAction("Index");
    }

}
