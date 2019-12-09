using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Controllers
{
     [Route("api/[controller]")]
    [ApiController]

    public class SalaController : ControllerBase
    {
        SalaRepositorio repositorio = new SalaRepositorio();

        /// <summary>
        /// Método Get, é para fazer a requisição HTTP Sala onde transmite a informação identificada na URL.
        /// </summary>
        /// <returns>Está retornando ListaDeSala</returns>
        
        // [Authorize(Roles="Administrador")]
        [HttpGet]
        public async Task<ActionResult<List<Sala>>> Get()
        {
            List<Sala> ListaDeSala = await repositorio.Get();
            if (ListaDeSala == null)
            {
                return NotFound();
            }
            return ListaDeSala;
        }
        /// <summary>
        /// Método Get(id) é para fazer a requisição HTTP Sala pelo Id, onde transmite a informação identificada na URL.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Está retornando salaRetornada</returns>

        // [Authorize(Roles="Administrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Sala>> Get(int id)
        {
            Sala salaRetornada = await repositorio.Get(id);
            if (salaRetornada == null)
            {
                return NotFound();
            }
            return salaRetornada;
        }

    }
}