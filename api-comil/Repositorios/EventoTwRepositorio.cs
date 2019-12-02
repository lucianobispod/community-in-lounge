using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using api_comil.Interfaces;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Repositorios
{
    public class EventoTwRepositorio : IEventoTw
    {
        communityInLoungeContext db = new communityInLoungeContext();

        public async Task<ActionResult<EventoTw>> Get(int id)
        {
            return await db.EventoTw
            .Where(w => w.DeletadoEm == null)
            .Where(w => w.EventoId == id)
            .Include(i => i.Categoria)
            .FirstOrDefaultAsync();
        }

        public async Task<ActionResult<EventoTw>> GetPrivateById(int id)
        {
            return await db.EventoTw
            .Where(w => w.DeletadoEm == null)
            .Where(w => w.EventoId == id)
            .Where(w => w.Publico == "nao")
            .Include(i => i.Categoria)
            .FirstOrDefaultAsync();
        }

        public async Task<ActionResult<List<EventoTw>>> GetPublic()
        {
            var eventos = await db.EventoTw
           .Where(w => w.DeletadoEm == null)
           .Where(w => w.Publico.ToLower() == "sim")
           .Include(i => i.Categoria)
           .ToListAsync();

            foreach (var item in eventos)
            {
                item.Categoria.Evento = null;
                item.Categoria.EventoTw = null;
            }

            return eventos;
        }
        public async Task<ActionResult<List<EventoTw>>> GetPrivate()
        {
            var eventos = await db.EventoTw
           .Where(w => w.DeletadoEm == null)
           .Where(w => w.Publico.ToLower() == "nao")
           .Include(i => i.Categoria)
           .ToListAsync();

            foreach (var item in eventos)
            {
                item.Categoria.Evento = null;
                item.Categoria.EventoTw = null;
            }

            return eventos;
        }



        public async Task<ActionResult<List<EventoTw>>> GetMounth(int mes)
        {
            var eventos = await db.EventoTw
                            .Where(w => w.DeletadoEm == null)
                            .Where(w => w.EventoData.Year == DateTime.Now.Year)
                            .Where(w => w.EventoData.Month == mes)
                            .OrderBy(o => o.EventoData)
                            .Include(i => i.Categoria)
                            .ToListAsync();

            foreach (var item in eventos)
            {
                item.Categoria.Evento = null;
                item.Categoria.EventoTw = null;
            }

            return eventos;
        }

        public async Task<ActionResult<List<EventoTw>>> ExistaData(DateTime data)
        {
            var eventos = await db.EventoTw
           .Where(w => w.DeletadoEm == null)
           .Where(w => w.EventoData == data)
           .Include(i => i.Categoria)
           .ToListAsync();

            foreach (var item in eventos)
            {
                item.Categoria.Evento = null;
                item.Categoria.EventoTw = null;
            }

            return eventos;
        }

        public async Task<ActionResult<EventoTw>> Post(EventoTw evento)
        {
            var categoria = await db.Categoria.Where(c => c.CategoriaId == evento.CategoriaId).FirstOrDefaultAsync();

            var sala = await db.Sala.Where(s => s.SalaId == evento.SalaId).FirstOrDefaultAsync();

            if (categoria != null && sala != null)
            {
                db.Add(evento);
                await db.SaveChangesAsync();
                return evento;
            }
            else
            {
                return null;
            }
        }

        public async Task<ActionResult<EventoTw>> Put(EventoTw evento)
        {
            var categoria = await db.Categoria.Where(c => c.CategoriaId == evento.CategoriaId).FirstOrDefaultAsync();

            var sala = await db.Sala.Where(s => s.SalaId == evento.SalaId).FirstOrDefaultAsync();

            if (categoria != null && sala != null)
            {
                db.Entry(evento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return evento;
            }
            else
            {
                return null;
            }
        }

        public async Task<ActionResult<EventoTw>> Delete(EventoTw evento)
        {
            try
            {
                // Mensagem(evento.EmailContato);
                await db.SaveChangesAsync();
                return evento;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Mensagem(string email)
        {
            try
            {
                // Estancia da Classe de Mensagem
                MailMessage _mailMessage = new MailMessage();
                // Remetente
                _mailMessage.From = new MailAddress(email);

                // Destinatario seta no metodo abaixo

                //Contrói o MailMessage
                _mailMessage.CC.Add(email);
                _mailMessage.Subject = "TESTELIGHT CODE XP";
                _mailMessage.IsBodyHtml = true;
                _mailMessage.Body = "<b>Olá Tudo bem ??</b><p>Teste Parágrafo</p>";

                //CONFIGURAÇÃO COM PORTA
                SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

                //CONFIGURAÇÃO SEM PORTA
                // SmtpClient _smtpClient = new SmtpClient(UtilRsource.ConfigSmtp);

                // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação);
                _smtpClient.UseDefaultCredentials = false;

                _smtpClient.Credentials = new NetworkCredential("communitythoughtworks@gmail.com", "tw.123456");

                _smtpClient.EnableSsl = true;

                _smtpClient.Send(_mailMessage);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}