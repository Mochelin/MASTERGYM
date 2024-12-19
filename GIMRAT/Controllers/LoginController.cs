using Gimrat.Data;
using Gimrat.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GIMRAT.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioData _usuarioData;
        public LoginController(UsuarioData usuarioData)
        {
            _usuarioData = usuarioData;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string correo, string clave)
        {
            if (correo == null || clave == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            Usuario usuario_encontrado = new Usuario();
            usuario_encontrado = await _usuarioData.Obtener(correo, clave);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            ViewData["Mensaje"] = null;

            //aqui guarderemos la informacion de nuestro usuario
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario_encontrado.nombre_usuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.id_usuario.ToString()),
                    new Claim(ClaimTypes.Role,"Administrador")
                };


            return RedirectToAction("Index", "Home");
        }
    }
}
