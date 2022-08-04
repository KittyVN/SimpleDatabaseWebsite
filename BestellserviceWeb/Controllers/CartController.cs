using BestellserviceWeb.Data;
using BestellserviceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BestellserviceWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly BestellserviceDBContext _context;
        public List<TblKunde> kundenCart = new List<TblKunde>();

        public CartController(BestellserviceDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(kundenCart);
        }
    }
}
