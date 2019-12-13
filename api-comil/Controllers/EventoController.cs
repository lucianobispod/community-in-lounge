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
        UploadRepositorio _uploadRepo = new UploadRepositorio();


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
                    return StatusCode(200, "Ocorreu um problema na hora de enviar email");
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
                    return StatusCode(404, "Evento não encontrado");
                }

                return evento;

            }
            catch (System.Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> GetAll()
        {
            try
            {
                var evento = await EventoRep.Get();

                if (evento == null)
                {
                    return StatusCode(404, "Eventos não encontrados");
                }

                return evento;

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Evento>> Delete(int id)
        {
            try
            {
                var evento = await EventoRep.Get(id);

                if (evento == null) return StatusCode(404, "Evento não encontrado");
                if (evento.StatusEvento == "Realizado") return StatusCode(403, "No momento não é possivel excluir esse evento - Realizado");
                if (evento.StatusEvento == "Recusado") return StatusCode(403, "No momento não é possivel excluir esse evento - Recusado");
                if (evento.StatusEvento == "Aprovado") return StatusCode(403, "No momento não é possivel excluir esse evento - Aprovado");

                if (evento.StatusEvento == "Pendente")
                {
                    await EventoRep.Delete(evento);
                }


                return evento;

            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpGet("pendenteMes/{mes}")]
        public async Task<ActionResult<List<Evento>>> PendingMounth(int mes)
        {
            if (mes > 12 || mes <= 0)
            {
                return StatusCode(400, "Esse mês não é valido");
            }
            return await EventoRep.PendingMounth(mes);
        }


        [HttpPut("realize/{id}")]
        public async Task<ActionResult<Evento>> Realize(int id)
        {
            var evento = EventoRep.Get(id).Result;
            if (evento == null) return StatusCode(404, "Esse evento não foi encontrado");

            return await EventoRep.Realize(evento);
        }




        /// <summary>
        /// Método de eventos pendentes do usuário
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Retorna os eventos pendenetes do usuário</returns>
        [HttpGet("PendentesUsuario/{id}")]
        public async Task<ActionResult<List<Evento>>> PendingUser(int id)
        {
            try
            {
                var eventos = await EventoRep.PendingUser(id);
                if (eventos == null) return StatusCode(404, "Eventos não encontrados");

                return eventos;
            }
            catch (System.Exception)
            {

                throw;
            }

        }




        /// <summary>
        /// Método de busca dos eventos já realizados pelo usuário
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Eventos realizados pelo usuário</returns>
        [HttpGet("RealizadosUsuario/{id}")]
        public async Task<ActionResult<List<Evento>>> RealizeUser(int id)
        {

            return await EventoRep.RealizeUser(id);
        }




        /// <summary>
        /// Método de busca dos eventos do usuário que foram aprovados
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <returns>Eventos do usuário que estão aprovados</returns>
        [HttpGet("AprovadosUsuario/{id}")]
        public async Task<ActionResult<List<Evento>>> ApprovedUser(int id)
        {

            return await EventoRep.ApprovedUser(id);
        }


        /// <summary>
        /// Método para atualização de dados de um evento
        /// </summary>
        /// <param name="id">Id do evento</param>
        /// <param name="evento">Objeto evento</param>
        /// <returns>Evento com dados atualizados</returns>

        [HttpPut("{id}")]
        public async Task<ActionResult<Evento>> Put(int id, Evento evento)
        {
            if (id != evento.EventoId) return StatusCode(400);

            Evento eventoValido = EventoRep.Get(id).Result;

            if (eventoValido == null) return StatusCode(404);
            if (eventoValido.StatusEvento == "Reprovado") return StatusCode(403, "Não é possivel atualizar, esse evento foi reprovado");
            if (eventoValido.StatusEvento == "Realizado") return StatusCode(403, "Não é possivel atualizar, esse evento já foi realizado");

            if (eventoValido.StatusEvento == "Aprovado")
            {
                eventoValido.UrlEvento = evento.UrlEvento;

                return await EventoRep.Update(eventoValido);
            }
            else
            {
                return StatusCode(403, "Não é possivel atualizar ainda pendente");
            }

        }




        // ///adm
        /// <summary>
        /// Método de busca dos eventos aprovados pelo respectivo administrador
        /// </summary>
        /// <param name="id">Id do administrador</param>
        /// <returns>Eventos que o respectivo administrador aceitou</returns>
        [HttpGet("Aprovados/{id}")]
        public async Task<ActionResult<List<Evento>>> MyEventsAccept(int id)
        {
            return await EventoRep.MyEventsAccept(id);
        }


        // //ADM
        // /// <summary>
        // /// Método dos eventos recusados pelo respectivo administrador
        // /// </summary>
        // /// <param name="id">Id do administrador</param>
        // /// <returns>Eventos recusados do respectivo administrador</returns>
        // [HttpGet("Recusados/{id}")]
        // public async Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsReject(int id)
        // {

        //     return await EventoRep.MyEventsReject(id);
        // }


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




        [HttpPut("{id}/uploadFoto")]
        public async Task<ActionResult<Evento>> Put(int id)
        {
            var evento = await EventoRep.Get(id);
            if (evento == null)
            {
                return NotFound("Evento não encontrado");
            }

            if( evento.StatusEvento == "Realizado" || evento.DeletadoEm != null){
                return StatusCode(403,"Não é possivel fazer essa operação");
            }

            try
            {
                var arquivo = Request.Form.Files[0];
                var caminho = _uploadRepo.Upload(arquivo, "Imagens/Evento");


                evento.Foto = caminho;

                return await EventoRep.Update(evento);

            }
            catch (DbUpdateConcurrencyException)
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