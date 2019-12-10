using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using api_comil.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoTwController : ControllerBase
    {
        EventoTwRepositorio repositorio = new EventoTwRepositorio();
        UploadRepositorio _uploadRepo = new UploadRepositorio();

        [HttpGet("public")]
        public async Task<ActionResult<List<EventoTw>>> GetPublic()
        {

            var eventos = await repositorio.GetPublic();

            if (eventos == null) return NotFound();

            return eventos;
        }

        // Adm
        [HttpGet("private")]
        public async Task<ActionResult<List<EventoTw>>> GetPrivate()
        {

            var eventos = await repositorio.GetPrivate();

            if (eventos == null) return NotFound();

            return eventos;
        }

        //adm
        [HttpGet("{id}")]
        public async Task<ActionResult<EventoTw>> GetById(int id)
        {

            var evento = await repositorio.Get(id);

            if (evento.Result == null) return NotFound();

            return evento;
        }

        //adm
        [HttpGet("privado/{id}")]
        public async Task<ActionResult<EventoTw>> Private(int id)
        {

            var evento = await repositorio.GetPrivateById(id);

            if (evento.Result == null) return NotFound();

            return evento;
        }

        [HttpGet("Mes/{mes}")]
        public async Task<ActionResult<List<EventoTw>>> GetMounth(int mes)
        {
            if (mes > 0 && mes < 13)
            {
                var evento = await repositorio.GetMounth(mes);

                if (evento.Value == null) return NotFound();

                return evento.Value;

            }else{
                return BadRequest("Mês inválido");
            }
        }

        //adm
        [HttpPost]
        public async Task<ActionResult<EventoTw>> Post(EventoTw evento)
        {

            if (!ValidaEnderecoEmail(evento.EmailContato))
            {
                return BadRequest("Email inválido");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (evento.EventoData.Month < DateTime.Now.Month) return BadRequest("No momento não é possivel voltar no tempo (Mês)");
            if (evento.EventoData.Day < DateTime.Now.Day) return BadRequest("No momento não é possivel voltar no tempo (Dia)");

            EventoRepositorio eventoRepositorio = new EventoRepositorio();

            List<EventoTw> dataIgual = repositorio.ExistaData(evento.EventoData).Result.Value;
            List<EventoTw> dataIgualComunidade = repositorio.ExistaData(evento.EventoData).Result.Value;


            if (dataIgual.Count > 0 || dataIgualComunidade.Count > 0) return BadRequest("Evento já cadastrado com essa data " + dataIgual.Count + dataIgualComunidade.Count );



            return await repositorio.Post(evento);

        }

        // adm
        [HttpPut("{id}")]
        public async Task<ActionResult<EventoTw>> Put(EventoTw update, int id)
        {

            var evento = await repositorio.Get(id);
            if (evento.Value == null) return NotFound();

            evento.Value.Nome = update.Nome;
            evento.Value.Descricao = update.Descricao;
            evento.Value.Horario = update.Horario;
            evento.Value.Diversidade = update.Diversidade;
            evento.Value.Coffe = update.Coffe;
            evento.Value.CategoriaId = update.CategoriaId;
            evento.Value.SalaId = update.SalaId;

            var sucess = await repositorio.Put(evento.Value);
            if (sucess == null) return BadRequest("Não foi possivel atualizar");

            return sucess;

        }


        [HttpPut("{id}/uploadFoto")]
        public async Task<ActionResult<EventoTw>> Upload(int id)
        {
            var evento = await repositorio.Get(id);
            if (evento.Value == null)
            {
                return NotFound("Evento não encontrado");
            }
            if (evento.Value.DeletadoEm != null)
            {
                return NotFound("Não é possivel fazer essa operação");
            }

            try
            {
                var arquivo = Request.Form.Files[0];
                var caminho = _uploadRepo.Upload(arquivo, "Imagens/Thoughtwoks");

                evento.Value.Foto = caminho;

                return await repositorio.Upload(evento.Value);

            }
            catch (Exception)
            {
                throw;
            }

        }

        // adm
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventoTw>> Delete(int id)
        {

            try
            {
                var evento = await repositorio.Get(id);

                if (evento.Value == null) return NotFound();
                if (evento.Value.DeletadoEm != null) return NotFound(" Evento já deletado");

                evento.Value.DeletadoEm = DateTime.Now;
                return await repositorio.Delete(evento.Value);

            }
            catch (Exception)
            {

                throw;
            }

        }



        private bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
                return expressaoRegex.IsMatch(enderecoEmail);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}