
using Microsoft.AspNetCore.Mvc;
using Scarpe.Models;
using System.Diagnostics;

namespace Scarpe.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var shoes = StaticDb.GetAll();
            return View(shoes);
        }

        public IActionResult Details(int? id)
        {
            var shoe = StaticDb.GetById(id);
            return View(shoe);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var shoe = StaticDb.GetById(id);
            return View(shoe);
        }

        //[ActionName("Edit")]
        [HttpPost]
        public IActionResult Edit(Shoe shoe)
        {
            var newShoe = StaticDb.Modify(shoe);
            return RedirectToAction("Index");
        }

        //cambiato il nome
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deletedShoe = StaticDb.HardDelete(id);
            return RedirectToAction("Index");
        }
    }
}

