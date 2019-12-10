using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunidadeController : ControllerBase
    {

        // /// <summary>
        // /// Método resposável por fazer busca de todas as comunidade no banco de dados
        // /// </summary>
        // /// <returns>Todas as comunidades cadastradas no banco de dados</returns>
        ComunidadeRepositorio repositorio = new ComunidadeRepositorio();
        UploadRepositorio _uploadRepo = new UploadRepositorio();

        [AllowAnonymous]
        [HttpGet("byuser/{id}")]
        public async Task<ActionResult<List<Comunidade>>> GetByUser(int id)
        {
            try
            {

                var Comunidades = await repositorio.GetByUser(id);

                if (Comunidades == null)
                {
                    return NotFound();
                }
                else
                {
                    return Comunidades;
                }


            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // /// <summary>
        // /// Método resposável por fazer busca de uma comunidade expecífica através do parâmetro - ID
        // /// </summary>
        // /// <param name="id">Recebe i ID da comunidade</param>
        // /// <returns>Comundade correspondente ao ID digitado</returns>
        // [Authorize]   
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Comunidade>> Get(int id)
        // {

        //     try
        //     {
        //         var comunidade = await repositorio.Get(id);

        //         if (comunidade != null)
        //         {
        //             return comunidade;
        //         }
        //         else
        //         {
        //             return NotFound();
        //         }
        //     }
        //     catch (System.Exception)
        //     {

        //         throw;
        //     }
        // }




        // /// <summary>
        // /// Método resposável por fazer alteração na comunidade encontrada através do ID ou nome de uma comunidade 
        // /// </summary>
        // /// <param name="id">Recebe ID da comunidade</param>
        // /// <param name="comunidade">Recebe o objeto comunidade</param>
        // /// <returns>Comunidade alterada</returns>
        // [Authorize]
        [HttpPut("{id}/usuario/{userid}")]
        public async Task<ActionResult<Comunidade>> Put(int id, int userid, Comunidade comunidade)
        {

            try
            {

                var comunidadeRetornada = await repositorio.Get(id);

                if (comunidadeRetornada == null) return NotFound();

                if (comunidadeRetornada.ResponsavelUsuarioId != userid) return Forbid();

                if (comunidadeRetornada.DeletadoEm != null) return NotFound();


                if(comunidadeRetornada.Nome != null) comunidadeRetornada.Nome = comunidade.Nome;
                if(comunidadeRetornada.Descricao != null) comunidadeRetornada.Descricao = comunidade.Descricao;
                if(comunidadeRetornada.EmailContato != null) comunidadeRetornada.EmailContato = comunidade.EmailContato;
                if(comunidadeRetornada.TelefoneContato != null) comunidadeRetornada.TelefoneContato = comunidade.TelefoneContato;
                
                if(comunidadeRetornada.Foto != null) comunidadeRetornada.Foto = comunidade.Foto;


                return await repositorio.Put(comunidadeRetornada);

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

        }


        // /// <summary>
        // /// Método resposável por deletar uma comunidade, mudando seu status deixando-a desabilitada
        // /// </summary>
        // /// <param name="id">Id da comunidade</param>
        // /// <returns>Comunidade que foi desativada do sistema</returns>
        // [Authorize]   
        [HttpDelete("{id}/usuario/{userid}")]

        public async Task<ActionResult<Comunidade>> Delete(int id, int userid)
        {

            try
            {
                var comunidadeRetornada = await repositorio.Get(id);

                if (comunidadeRetornada == null) return NotFound();

                if (comunidadeRetornada.DeletadoEm != null) return NotFound(" Comunidade já deletada");

                if (comunidadeRetornada.ResponsavelUsuarioId != userid) return Forbid();

                comunidadeRetornada.DeletadoEm = DateTime.Now;
                await repositorio.Delete(comunidadeRetornada);
                return comunidadeRetornada;


            }
            catch (System.Exception)
            {

                throw;
            }

        }


        // /// <summary>
        // /// Método responsável por cadastrar uma comunidade
        // /// </summary>
        // /// <param name="comunidade">Objeto comunidade</param>
        // /// <returns>Comunidade cadastrada</returns>
        // [Authorize]
        [HttpPost]
        public async Task<ActionResult<Comunidade>> Post(Comunidade comunidade)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {

                    var verifica = await repositorio.Get(comunidade.Nome);

                    if (verifica != null)
                    {
                        return BadRequest("Já existe uma comunidade com esse nome");
                    }
                    else
                    {
                        await repositorio.Post(comunidade);
                        return comunidade;
                    }

                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }





   [HttpPut("{id}/uploadFoto")]
        public async Task<ActionResult<Comunidade>> Put(int id)
        {
            var comunidade = await repositorio.Get(id);
            if (comunidade == null)
            {
                return NotFound("comunidade não encontrada");
            }

            if( comunidade.DeletadoEm != null){
                return StatusCode(403,"Não é possivel fazer essa operação");
            }

            try
            {
                var arquivo = Request.Form.Files[0];
                var caminho = _uploadRepo.Upload(arquivo, "Imagens/Comunidade");


                comunidade.Foto = caminho;

                return await repositorio.Put(comunidade);

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

        }










    }
}
