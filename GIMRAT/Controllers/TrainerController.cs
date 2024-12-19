using Gimrat.Data;
using Gimrat.Entidades;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GIMRAT.Controllers
{
    public class TrainerController : Controller
    {
        // GET: TrainerController

        private readonly TrainerData _trainerData;
        public TrainerController(TrainerData trainerData)
        {
            _trainerData = trainerData;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult pagos()
        {
            return View();
        }
        // GET: TrainerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainerController/Create
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] trainer objeto_trainer)
        {
            string respuesta = await _trainerData.Crear(objeto_trainer);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] trainer objeto_trainer)
        {
            string respuesta = await _trainerData.Editar(objeto_trainer);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int Id)
        {
            string respuesta = await _trainerData.Eliminar(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<trainer> lista = await _trainerData.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }


        [HttpPost]
        public async Task<IActionResult> CrearPago([FromBody] pagoTrainer objeto_pago)
        {
            if (objeto_pago == null)
            {
                return BadRequest("El objeto recibido es nulo. Verifica el JSON enviado.");
            }

            // Opcional: Registra los datos recibidos para depuración
            Console.WriteLine(JsonConvert.SerializeObject(objeto_pago));

            string respuesta = await _trainerData.RegistrarPago(objeto_pago);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }




        [HttpGet]
        public async Task<IActionResult> ObtenerTrainer(string rut_trainer)
        {
            try
            {
                // Llamar al método Obtener de la clase TrainerData
                var trainer = await _trainerData.Obtener(rut_trainer);

                // Devolver los datos como JSON con un estado 200 OK
                return StatusCode(StatusCodes.Status200OK, new { data = trainer });
            }
            catch (Exception ex)
            {
                // Manejar la excepción (registrarla, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        public async Task<IActionResult> ObtenerPagosTrainer(string rut_trainer)
        {
            try
            {
                // Obtener los pagos del entrenador utilizando la data
                var pagos = await _trainerData.ObtenerPagosTrainer(rut_trainer);

                // Devolver los datos como JSON con un estado 200 OK
                return StatusCode(StatusCodes.Status200OK, new { data = pagos });
            }
            catch (Exception ex)
            {
                // Manejar la excepción (registrarla, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }




        // POST: TrainerController/Create
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

        // GET: TrainerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainerController/Edit/5
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

        // GET: TrainerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainerController/Delete/5
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
