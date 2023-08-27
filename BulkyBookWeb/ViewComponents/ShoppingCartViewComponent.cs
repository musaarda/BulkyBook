using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.ViewComponents;

public class ShoppingCartViewComponent : ViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync()
    {

        var claimsIdentity = (ClaimsIdentity)User.Identity;

        var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        var userId = claims.Value.ToString();

        return View(0);

    }

}
