namespace ZakaraiMe.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class JourneysController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MapBox()
        {
            return View();
        }
    }
}