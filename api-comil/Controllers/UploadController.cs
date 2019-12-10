using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Controllers {


    [Route ("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase {
        

        FotoRepositorio _repositorio = new FotoRepositorio();
        UploadRepositorio _uploadRepo  = new UploadRepositorio ();


        [HttpPut("envio")]
        public async Task<ActionResult<Usuario>> Put ([FromForm] Usuario usuario)  {
            // if (id != usuario.UsuarioId) {
            //     return BadRequest ();
            // }
            try {
                var arquivo = Request.Form.Files[0];

                usuario.Nome = Request.Form["nome"].ToString();
                usuario.Email = Request.Form["email"].ToString();
                usuario.Genero = Request.Form["genero"].ToString();
                usuario.Telefone = Request.Form["telefone"].ToString();
                usuario.Senha = Request.Form["senha"].ToString();
                usuario.TipoUsuarioId = int.Parse(Request.Form["tipousuarioid"]);
            

                usuario.Foto = _uploadRepo.Upload(arquivo, "Imagens/FotoUsuario");

                await _repositorio.Salvar(usuario);


            } catch (DbUpdateConcurrencyException) {
               var usuario_valida = await _repositorio.BuscarPorID(usuario.UsuarioId);
                if (usuario_valida == null) {
                    return NotFound ("Ong não encontrada");
                } else {
                    throw;
                }

            }
            return NoContent();
        }
    }
}