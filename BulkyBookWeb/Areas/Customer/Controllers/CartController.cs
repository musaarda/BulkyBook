using System.Security.Claims;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartVM ShoppingCartVM { get; set; }
    public int OrderTotal { get; set; }

    public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVM = new ShoppingCartVM()
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, includeProperties: "Product")
        };

        foreach (var cart in ShoppingCartVM.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product);
            ShoppingCartVM.CartTotal += cart.Price * cart.Count;
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        // var claimsIdentity = (ClaimsIdentity)User.Identity;
        // var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        // ShoppingCartVM = new ShoppingCartVM()
        // {
        //     ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, includeProperties: "Product")
        // };

        // foreach (var cart in ShoppingCartVM.ListCart)
        // {
        //     cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product);
        //     ShoppingCartVM.CartTotal += cart.Price * cart.Count;
        // }

        return View(ShoppingCartVM);
    }

    public IActionResult Plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

        if (cart.Count <= 1)
        {
            _unitOfWork.ShoppingCart.Remove(cart);
        }
        else
        {
            _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.Remove(cart);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    private double GetPriceBasedOnQuantity(double quantity, Product product)
    {
        if (quantity <= 50)
        {
            return product.Price;
        }
        else if (quantity <= 100)
        {
            return product.Price50;
        }
        else
        {
            return product.Price100;
        }
    }

}
