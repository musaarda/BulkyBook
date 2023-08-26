using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private readonly ILogger<CompanyController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(ILogger<CompanyController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET
    public IActionResult Upsert(int? id)
    {
        Company company = new();

        if (id == null || id == 0)
        {
            // Create Product
            // ViewBag.CategoryList = CategoryList;
            // ViewData["CoverTypeList"] = CoverTypeList;

            return View(company);
        }
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            // Update the compnay
            return View(company);
        }
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            if (obj.Id == 0)
            {
                _unitOfWork.Company.Add(obj);
                TempData["Success"] = "Company created successfully.";
            }
            else
            {
                _unitOfWork.Company.Update(obj);
                TempData["Success"] = "Company updated successfully.";
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var objFromDb = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
        if (objFromDb == null)
            return NotFound();

        return View(objFromDb);
    }

    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
            return NotFound();

        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        TempData["Success"] = "Company deleted successfully.";
        return RedirectToAction("Index");
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
    }

    [HttpDelete]
    public IActionResult DeleteApi(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
            return Json(new { success = false, message = "Error while deleting." });

        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successfull." });
    }
    #endregion

}
