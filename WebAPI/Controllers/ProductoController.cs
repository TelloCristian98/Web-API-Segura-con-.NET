using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly DbapiContext _dbContext;

        public ProductoController(DbapiContext dbContext) {  _dbContext = dbContext; }

        [HttpGet]
        public IActionResult Get() {
            List<Producto> products = new List<Producto>();
            try
            {
                products = _dbContext.Productos.Include(c => c.objCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = products });
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = products });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetProductoId(int id)
        {
            Producto objProducto = _dbContext.Productos.Find(id);
            if (objProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                objProducto = _dbContext.Productos.Include(c => 
                    c.objCategoria).Where(p => p.IdProducto == id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = objProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = objProducto });
            }
        }

        [HttpPost]
        public IActionResult SaveProduct([FromBody]Producto objProducto)
        {
            try
            {
                _dbContext.Productos.Add(objProducto);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Producto agregado correctamente", Response = objProducto });
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult EditProducto([FromBody] Producto oProducto)
        {
            Producto objProducto = _dbContext.Productos.Find(oProducto.IdProducto);
            if (objProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                objProducto.CodigoBarra = oProducto.CodigoBarra is null ? objProducto.CodigoBarra : oProducto.CodigoBarra;
                objProducto.Descripcion = oProducto.Descripcion is null ? objProducto.Descripcion : oProducto.Descripcion;
                objProducto.Marca = oProducto.Marca is null ? objProducto.Marca : oProducto.Marca;
                objProducto.IdCategoria = oProducto.IdCategoria is null ? objProducto.IdCategoria : oProducto.IdCategoria;
                objProducto.Precio = oProducto.Precio is null ? objProducto.Precio : oProducto.Precio;
                _dbContext.Productos.Update(objProducto);
                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Producto actualizado correctamente", response = objProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = objProducto });
            }
        }

        [HttpPost]
        [Route("Delete/{id:int}")]
        [Authorize]
        public IActionResult DeleteProduct(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity);
            if (!rToken.succes) return StatusCode(StatusCodes.Status200OK, new { mensaje = "No has iniciado sesion" });
            Usuario usuario = rToken.result;
            if (usuario.rol != "administrador")
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "No tienes permiso para eliminar" });
            }
            else
            {
                Producto objProducto = _dbContext.Productos.Find(id);
                if (objProducto == null)
                {
                    return BadRequest("Producto no encontrado");
                }

                try
                {
                    _dbContext.Productos.Remove(objProducto);
                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Producto eliminado correctamente" });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
                }
            }

            
        }

    }
}
