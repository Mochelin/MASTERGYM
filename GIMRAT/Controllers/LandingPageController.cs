using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GIMRAT.Controllers
{
    public class LandingPageController : Controller
    {
        // GET: Landing
        public IActionResult Index()
        {
            return View();
        }

        // GET: Landing/Details/5
        public ActionResult contact()
        {
            return View();
        }
    

        // GET: Landing/Create
        public ActionResult classes()
        {
            return View();
        }
      

        // POST: Landing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Landing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Landing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Landing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Landing/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
