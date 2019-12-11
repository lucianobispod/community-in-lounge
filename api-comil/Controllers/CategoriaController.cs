using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        CategoriaRepositorio repositorio = new CategoriaRepositorio();


        // [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            try
            {
                var Categorias = await repositorio.Get();

                //atribui o valor nulo para todos eventos que estão aninhados a categoria
                foreach (var item in Categorias)
                {
                    item.Evento = null;
                    item.EventoTw = null;
                }

                if (Categorias == null)
                {
                    return NotFound();
                }

                return Categorias;

            }
            catch (Exception)
            {
                return Forbid();
                throw;
            }


        }



        [HttpGet("{nome}")]
        public async Task<ActionResult<List<Categoria>>> Get(string nome)
        {
            try
            {
                var categorias = await repositorio.Search(nome);

                if (categorias == null)
                {
                    return NotFound();
                }
                
                //atribui o valor nulo para todos eventos que estão aninhados a categoria
                foreach (var item in categorias)
                {
                    item.Evento = null;
                    item.EventoTw = null;
                }

                return categorias;

            }
            catch (Exception)
            {
                throw;
            }


        }




        // [Authorize(Roles ="Administrador")]
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria nomeCategoria)
        {
            try
            {
                var categoria = repositorio.Get(nomeCategoria);
                var cat = categoria.Result;

                if (cat == null)
                {

                    await repositorio.Post(nomeCategoria);
                    return nomeCategoria;

                }
                else
                {

                    if (cat.DeletadoEm != null)
                    {

                        cat.DeletadoEm = null;
                        await repositorio.Put(cat);
                        return cat;
                    }
                    else
                    {
                        return BadRequest("Categoria já existe");
                    }
                }

            }
            catch (Exception)
            {
                return Forbid();
                throw;
            }



        }


        // [Authorize(Roles ="Administrador")]
        [HttpDelete("{id}")]

        public async Task<ActionResult<Categoria>> Delete(int id)
        {


            try
            {
                var categoria = await repositorio.Get(id);

                if (categoria != null)
                {
                    categoria.DeletadoEm = DateTime.Now;
                    await repositorio.Delete(categoria);

                    categoria.Evento = null;
                    categoria.EventoTw = null;

                    return categoria;
                }
                else
                {
                    return NotFound("Categoria não encontrada");
                }
            }
            catch (Exception)
            {
                return Forbid();
                throw;
            }


        }




        // [Authorize(Roles ="Administrador")]
        // [HttpPut("{id}")]

        // public async Task<ActionResult<Categoria>> Put(int id, [FromBody]Categoria update)
        // {
        //     try
        //     {

        //         if (update.CategoriaId == id)
        //         {
        //             var categoria = await repositorio.Get(id);

        //             var nomeExistente = await repositorio.Get(update);
        //             if (nomeExistente != null)
        //             {
        //                 return BadRequest("Já existe uma categoria com esse nome");
        //             }


        //             if (categoria != null)
        //             {
        //                 categoria.Nome = update.Nome;
        //                 return await repositorio.Put(categoria);
        //             }
        //             else
        //             {
        //                 return NotFound();
        //             }

        //         }else{
        //             return BadRequest();
        //         }
        //     }
        //     catch (System.Exception)
        //     {

        //         throw;
        //     }

        // }






    }
}