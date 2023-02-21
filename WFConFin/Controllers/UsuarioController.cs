using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;
using WFConFin.Services;

namespace WFConFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly WFConFinContext _context;
        private readonly TokenService _tokenService;

        public UsuarioController(WFConFinContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
        {
            var usuario = _context.Usuario.Where(x => x.login == usuarioLogin.login).FirstOrDefault();
            if(usuario == null)
            {
                return NotFound("Usuário inváldo");
            }

            string passwordHash = MD5Hash.CalcHash(usuarioLogin.password);
           

            if (usuario.password != passwordHash)
            {
                return BadRequest("Senha inválida");
            }

            var token = _tokenService.GerarToken(usuario);

            usuario.password = "";

            var result = new UsuarioResponse()
            {
                usuario = usuario,
                token = token
            };

            return Ok(result);
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            if (_context.Usuario == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuario.ToListAsync();
            var usuariosResult = new List<Usuario>();
            foreach (var usuario in usuarios)
            {
                usuariosResult.Add(new Usuario()
                {
                    id = usuario.id,
                    login = usuario.login,
                    nome = usuario.nome,
                    funcao = usuario.funcao
                });
            }

            return usuariosResult;
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            if (_context.Usuario == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuario.FindAsync(id);

            usuario.password = null;

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Gerente,Administrador")]
        public async Task<IActionResult> PutUsuario(Guid id, Usuario usuario)
        {
            if (id != usuario.id)
            {
                return BadRequest();
            }

            string passwordHash = MD5Hash.CalcHash(usuario.password);

            usuario.password = passwordHash;

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Gerente,Administrador")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'WFConFinContext.Usuario'  is null.");
            }

            string passwordHash = MD5Hash.CalcHash(usuario.password);

            usuario.password = passwordHash;

            _context.Usuario.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            usuario.password = null;

            return CreatedAtAction("GetUsuario", new { id = usuario.id }, usuario);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            if (_context.Usuario == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(Guid id)
        {
            return (_context.Usuario?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

