using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_comil.Models;
using api_comil.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class EventoController : ControllerBase
    {

        EventoRepositorio EventoRep = new EventoRepositorio();
        UsuarioRepositorio UsuarioRep = new UsuarioRepositorio();
        communityInLoungeContext db = new communityInLoungeContext();


        /// <summary>
        /// Método de cadastrar Evento 
        /// </summary>
        /// <returns>Retorna evento cadastrado</returns>

        [HttpPost]
        public async Task<ActionResult<Evento>> Post(Evento evento)
        {

            if (!ValidaEnderecoEmail(evento.EmailContato))
            {
                return BadRequest("Email inválido");
            }

            if (evento.EventoData.Month < DateTime.Now.Month) return BadRequest("No momento não é possivel voltar no tempo (Mês)");
            if (evento.EventoData.Day < DateTime.Now.Day) return BadRequest("No momento não é possivel voltar no tempo (Dia)");

            EventoTwRepositorio repositorioTw = new EventoTwRepositorio();

            List<Evento> dataIgual = EventoRep.ExistaData(evento.EventoData).Result.Value;
            List<EventoTw> dataIgualTw = repositorioTw.ExistaData(evento.EventoData).Result.Value;

            if (dataIgual.Count > 0 || dataIgualTw.Count > 0) return BadRequest("Evento já cadastrado com essa data " + dataIgual.Count + dataIgualTw.Count);

            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400);
                }
                else
                {
                    var eRtornado = await EventoRep.Post(evento);

                    if (eRtornado == null)
                    {
                        return StatusCode(400, "Evento não cadastrado, Verifique as chaves estrangeiras ");
                    }
                    else
                    {
                        return eRtornado;
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Método onde um administradoe aceita um evento
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <param name="idResp"> Id do resposável pelo evento</param>
        /// <returns>Evento aprovado</returns>
        // [Authorize(Roles = "Administrador")]
        [HttpPost("aceppt/{idEvento}/{idResp}")]
        public async Task<ActionResult<Evento>> Accept(int idEvento, int idResp)
        {

            var responsavel = await db.Usuario.FindAsync(idResp);

            var evento = await db.Evento.FindAsync(idEvento);

            if (responsavel == null || evento == null)
            {
                return StatusCode(400, "parametros passados inválidos");
            }
            else
            {

                try
                {
                    return await EventoRep.Accept(evento, responsavel.UsuarioId);
                }
                catch (System.Exception)
                {
                    return StatusCode(200, "Ocorreu um problema na hora de enviar email" );
                    throw;
                }

            }

        }


        /// <summary>
        /// Método onde um administrador recusa um evento
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <param name="idResp">Id do responsável pelo evento</param>
        /// <returns>Evento rejeitado</returns>
        // [Authorize(Roles = "Administrador")]
        [HttpPost("rejetc/{idEvento}/{idResp}")]
        public async Task<ActionResult<Evento>> Reject(int idEvento, int idResp)
        {
            var responsavel = await UsuarioRep.Get(idResp);

            var evento = await EventoRep.GetExistEvent(idEvento);
            if (responsavel == null || evento == null)
            {
                return StatusCode(400, "Esse evento já foi aprovado");
            }
            else
            {
                try
                {
                    return await EventoRep.Reject(evento, responsavel.UsuarioId);
                }
                catch (System.Exception)
                {
                    return StatusCode(200, "Não foi possivel enviar email");
                    throw;
                }
            }

        }





        /// <summary>
        /// Método de busca de um evento através de um Id
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Comunidade correspondente ao Id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            try
            {
                var evento = await EventoRep.Get(id);

                if (evento == null)
                {
                    return NotFound();
                }

                return evento;

            }
            catch (System.Exception)
            {

                throw;
            }
        }


















































        /// <summary>
        /// Método de busca de eventos do mês correspondente
        /// </summary>
        /// <returns>eventos do mês</returns>
        [HttpGet("Mes")]
        public async Task<ActionResult<List<Evento>>> Mouth()
        {
            return await EventoRep.Mounth();
        }



        //ADM
        /// <summary>
        /// Método de busca dos eventos que estão pendentes
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Eventos pendentes</returns>
        [HttpGet("Pendentes/{id}")]
        public async Task<ActionResult<List<ResponsavelEventoTw>>> PendingMounth(int id)
        {

            return await EventoRep.PendingMounth(id);
        }



















        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get()
        {
            return await EventoRep.Get();
        }





        /// <summary>
        /// Método para atualização de dados de um evento
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <param name="evento">Objeto evento</param>
        /// <returns>Evento com dados atualizados</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento evento)
        {
            if (id != evento.EventoId) return BadRequest();
            if (evento.DeletadoEm != null) return NotFound();


            try
            {
                Evento eventoValido = EventoRep.Get(id).Result;
                if (eventoValido == null)
                {
                    return NotFound();
                }
                else
                {
                    await EventoRep.Update(evento);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();

        }



        ///adm
        /// <summary>
        /// Método de busca dos eventos aprovados pelo respectivo administrador
        /// </summary>
        /// <param name="id">Id do administrador</param>
        /// <returns>Eventos que o respectivo administrador aceitou</returns>
        [HttpGet("Aprovados/{id}")]
        public async Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsAccept(int id)
        {
            return await EventoRep.MyEventsAccept(id);
        }


        //ADM
        /// <summary>
        /// Método dos eventos recusados pelo respectivo administrador
        /// </summary>
        /// <param name="id">Id do administrador</param>
        /// <returns>Eventos recusados do respectivo administrador</returns>
        [HttpGet("Recusados/{id}")]
        public async Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsReject(int id)
        {

            return await EventoRep.MyEventsReject(id);
        }


        // /// <summary>
        // /// Método de busca de evento através de uma categoria expecífica
        // /// </summary>
        // /// <param name="id">Id da categoria</param>
        // /// <returns>Eventos correspondentes à categoria</returns>
        // [HttpGet("Categoria/{id}")]
        // public async Task<ActionResult<List<Evento>>> EventByCategory(int id)
        // {

        //     return await EventoRep.EventByCategory(id);
        // }

        /// <summary>
        /// Método de eventos pendentes do usuário
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Retorna os eventos pendenetes do usuário</returns>
        [HttpGet("PendentesUsuario/{id}")]
        public async Task<ActionResult<List<ResponsavelEventoTw>>> PendingUser(int id)
        {

            return await EventoRep.PendingUser(id);
        }

        /// <summary>
        /// Método de busca dos eventos já realizados pelo usuário
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Eventos realizados pelo usuário</returns>
        [HttpGet("RealizadosUsuario/{id}")]
        public async Task<ActionResult<List<ResponsavelEventoTw>>> RealizeUser(int id)
        {

            return await EventoRep.RealizeUser(id);
        }

        /// <summary>
        /// Método de busca dos eventos do usuário que foram aprovados
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Eventos do usuário que estão aprovados</returns>
        [HttpGet("AprovadosUsuario/{id}")]
        public async Task<ActionResult<List<ResponsavelEventoTw>>> ApprovedUser(int id)
        {

            return await EventoRep.ApprovedUser(id);
        }

        //em vez de receber o id pega do token 


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