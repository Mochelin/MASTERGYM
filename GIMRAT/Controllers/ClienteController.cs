using Cliente.Data;
using Gimrat.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace GIMRAT.Controllers
{

    public class ClienteController : Controller
    {
        private readonly ClienteData _clienteData;
        public ClienteController(ClienteData clienteData)
        {
            _clienteData = clienteData;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<cliente> lista = await _clienteData.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] cliente objeto_cliente)
        {
            string respuesta = await _clienteData.Crear(objeto_cliente);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] cliente objeto_cliente)
        {
            string respuesta = await _clienteData.Editar(objeto_cliente);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int Id)
        {
            string respuesta = await _clienteData.Eliminar(Id);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
    }
}
