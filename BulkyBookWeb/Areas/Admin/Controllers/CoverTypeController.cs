using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly ILogger<CoverTypeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(ILogger<CoverTypeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();

        return View(objCoverTypeList);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "CoverType created successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
        if (coverTypeFromDb == null)
            return NotFound();

        return View(coverTypeFromDb);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = "CoverType updated successfully.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
        if (coverTypeFromDb == null)
            return NotFound();

        return View(coverTypeFromDb);
    }

    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
            return NotFound();

        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["Success"] = "CoverType deleted successfully.";
        return RedirectToAction("Index");
    }

}
