using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EstadoController : ControllerBase
    {
        private readonly WFConFinContext _context;

        public EstadoController(WFConFinContext context)
        {
            _context = context;
        }

        // GET: api/Estado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstado()
        {
          if (_context.Estado == null)
          {
              return NotFound();
          }
            return await _context.Estado.ToListAsync();
        }

        // GET: api/Estado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> GetEstado(string id)
        {
          if (_context.Estado == null)
          {
              return NotFound();
          }
            var estado = await _context.Estado.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }

        // PUT: api/Estado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Gerente,Administrador")]
        public async Task<IActionResult> PutEstado(string id, Estado estado)
        {
            if (id != estado.sigla)
            {
                return BadRequest();
            }

            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
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

        // POST: api/Estado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Gerente,Administrador")]
        public async Task<ActionResult<Estado>> PostEstado(Estado estado)
        {
          if (_context.Estado == null)
          {
              return Problem("Entity set 'WFConFinContext.Estado'  is null.");
          }
            _context.Estado.Add(estado);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EstadoExists(estado.sigla))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEstado", new { id = estado.sigla }, estado);
        }

        // DELETE: api/Estado/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteEstado(string id)
        {
            if (_context.Estado == null)
            {
                return NotFound();
            }
            var estado = await _context.Estado.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estado.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Estado/Pesquisa
        [HttpGet("Pesquisa")]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstadoPesquisa([FromQuery] string valor)
        {
            try
            {
                var lista = from o in await _context.Estado.ToListAsync()
                            where o.sigla.ToUpper().Contains(valor.ToUpper())
                            || o.nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de estado. Exceção: {e.Message}");
            }
            {

            }
        }

        private bool EstadoExists(string id)
        {
            return (_context.Estado?.Any(e => e.sigla == id)).GetValueOrDefault();
        }
    }
}
