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
    public class EventoRepositorio : IEvento
    {
        communityInLoungeContext db = new communityInLoungeContext();
        
        public async Task<Evento> Accept(Evento evento, int responsavel)
        {
            evento.StatusEvento = "Aprovado";
            db.Evento.Update(evento);

            db.ResponsavelEventoTw.Add(new ResponsavelEventoTw{ Evento = evento.EventoId, ResponsavelEvento = responsavel });
            Mensagem(evento.EmailContato);
            await db.SaveChangesAsync();
            return evento;
        }

        public async Task<ActionResult<List<ResponsavelEventoTw>>> ApprovedUser(int id)
        {
             return await db.ResponsavelEventoTw
                               .Include(w => w.EventoNavigation)
                               .ThenInclude(w => w.Comunidade)
                               .Where(w => w.EventoNavigation.StatusEvento == "Aprovado")
                               .Where(w =>  w.EventoNavigation.Comunidade.ResponsavelUsuarioId == id)
                               .ToListAsync();
        }

            public async Task<ActionResult<List<Evento>>> GetEventsByUser(int id){
             return await db.Evento
            .Where(w => w.DeletadoEm == null)
            .Where(w => w.StatusEvento == "aprovado")
            .Where(w => w.StatusEvento == "pendente")
            .Where(w => w.Comunidade.ResponsavelUsuarioId == id)
            .Include(i => i.Comunidade)
            .ToListAsync();
        }

        public Task<ActionResult> Delete()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResult<List<Evento>>> EventByCategory(int id)
        {
            var a = await db.Evento.Include(i => i.Categoria)
                           .Where(w => w.CategoriaId == id)
                           .ToListAsync();

            foreach (var item in a)
            {
                item.Categoria.Evento = null;
            }

            return a;
        }

        public async Task<ActionResult<List<Evento>>> Get()
        {
             var listEven = await db.Evento
            .Include(i => i.Categoria)
            .Include(i => i.Comunidade)
            .Where(w => w.StatusEvento == "Aprovado")
            .Where(w => w.DeletadoEm == null )
            .ToListAsync();

            foreach (var item in listEven)
            {
            item.Categoria.Evento = null;
            item.Comunidade.Evento = null;
            }

            return listEven;
        }


        public async Task<Evento> Get(int id)
        {
            var evento = await db.Evento
            .Include(i => i.Categoria)
            .Include(c => c.Comunidade)
            .Where(w => w.StatusEvento == "Aprovado")
            .Where(w => w.DeletadoEm == null )
            .FirstOrDefaultAsync(f => f.EventoId == id);

            return evento;
        }

        public async Task<ActionResult<List<Evento>>> Mounth()
        {
                return await db.Evento
                    .Where(w => w.DeletadoEm == null)
                    .Where(w => w.EventoData.Month == DateTime.Now.Month)
                    .OrderBy(o => o.EventoData)
                    .ToListAsync();
        }

        public async Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsAccept(int id)
        {
                return await db.ResponsavelEventoTw
                               .Include(w => w.EventoNavigation)
                               .Where(w => w.EventoNavigation.StatusEvento == "Aprovado")
                               .Where(w =>  w.ResponsavelEvento == id)
                               .ToListAsync();
        }

        public async Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsReject(int id)
        {
             return await db.ResponsavelEventoTw
                               .Include(w => w.EventoNavigation)
                               .Where(w => w.EventoNavigation.StatusEvento == "Recusado")
                               .Where(w =>  w.ResponsavelEvento == id)
                               .ToListAsync();
        }

        public async Task<ActionResult<List<ResponsavelEventoTw>>> PendingMounth(int id)
        {
              return await db.ResponsavelEventoTw
                               .Include(w => w.EventoNavigation)
                               .Where(w => w.EventoNavigation.StatusEvento == "Pendente")
                               .Where(w => w.EventoNavigation.EventoData.Month == DateTime.Now.Month)
                               .Where(w =>  w.ResponsavelEvento == id)
                               .ToListAsync();
        }

        public async Task<ActionResult<List<ResponsavelEventoTw>>> PendingUser(int id)
        {
            return await db.ResponsavelEventoTw
                               .Include(w => w.EventoNavigation)
                               .ThenInclude(w => w.Comunidade)
                               .Where(w => w.EventoNavigation.StatusEvento == "Pendente")
                               .Where(w =>  w.EventoNavigation.Comunidade.ResponsavelUsuarioId == id)
                               .ToListAsync();
        }

        public async Task<ActionResult<Evento>> Post(Evento evento)
        {
            Categoria categoria = await db.Categoria.Where(c => c.CategoriaId == evento.CategoriaId).FirstOrDefaultAsync();
            Sala sala = await db.Sala.Where(s => s.SalaId == evento.SalaId).FirstOrDefaultAsync();
            Comunidade comunidade = await db.Comunidade.Where(c => c.ComunidadeId == evento.ComunidadeId).FirstOrDefaultAsync();

            if(categoria != null &&  sala != null && comunidade != null)
            {
                   db.Add(evento);
                  await db.SaveChangesAsync(); 
            }
        
           return null;
        }

        public async Task<ActionResult<List<ResponsavelEventoTw>>> RealizeUser(int id)
        {
             return await db.ResponsavelEventoTw
                               .Include(w => w.EventoNavigation)
                               .ThenInclude(w => w.Comunidade)
                               .Where(w => w.EventoNavigation.StatusEvento == "Realizado")
                               .Where(w =>  w.EventoNavigation.Comunidade.ResponsavelUsuarioId == id)
                               .ToListAsync();
        }

        public async Task<Evento> Reject(Evento evento, int idResponsavel)
        {
            evento.StatusEvento = "Recusado"; 
            db.Evento.Update(evento);

           Mensagem(evento.EmailContato);

            db.ResponsavelEventoTw.Add(new ResponsavelEventoTw{ Evento = evento.EventoId, ResponsavelEvento = idResponsavel });

            await db.SaveChangesAsync();
            return evento;
        }

        private void Mensagem (string email) {
            try {
                // Estancia da Classe de Mensagem
                MailMessage _mailMessage = new MailMessage ();
                // Remetente
                _mailMessage.From = new MailAddress (email);

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

                _smtpClient.Send (_mailMessage);

            } catch (Exception ex) {
                throw ex;
            }
        }


        public async Task<ActionResult<Evento>> Update(Evento evento)
        {
        if(evento.StatusEvento == "Aprovado")
        {
            Evento eventoRetornado = await db.Evento.FindAsync(evento.EventoId);
            eventoRetornado.Nome = evento.Nome;
            db.Evento.Update(eventoRetornado);
        }else
        {
           db.Entry(evento).State = EntityState.Modified;
        }
        return evento;
        }

    }
}