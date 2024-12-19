using Gimrat.Data;
using Gimrat.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace GIMRAT.Controllers
{
    public class PlanesController : Controller
    {

        private readonly PlanesData _planesData;
        public PlanesController(PlanesData planesData)
        {
            _planesData = planesData;
        }
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Planes objeto_plan)
        {
            string respuesta = await _planesData.Crear(objeto_plan);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Planes objeto_plan)
        {
            string respuesta = await _planesData.Editar(objeto_plan);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }


        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Planes> lista = await _planesData.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }




        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }


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


        public ActionResult Edit(int id)
        {
            return View();
        }


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

        public ActionResult Delete(int id)
        {
            return View();
        }

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
