using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_comil.Interfaces;
using api_comil.Models;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Repositorios
{
    public class UsuarioRepositorio : IUsuario
    {

        communityInLoungeContext db = new communityInLoungeContext();
        public async Task<Usuario> Delete(Usuario usuario)
        {
            usuario.DeletadoEm = DateTime.Now;
            await db.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> Get(int id)
        {
            var usuario = await db.Usuario
            .Where(w => w.DeletadoEm == null)
            .Include(user => user.Comunidade)
            .FirstOrDefaultAsync(f => f.UsuarioId == id);
            
            foreach (var item in usuario.Comunidade)
            {
            item.Evento = null;
            }
            return usuario;
        }
        
        public async Task<Header> Header(int id)
        {
            return await db.Usuario
            .Where(w => w.DeletadoEm == null)
            .Where(w => w.UsuarioId == id)
            .Select(h => new Header{
                Nome = h.Nome,
                Foto = h.Foto
            }).SingleOrDefaultAsync();
            
        }

        public async Task<List<Usuario>> GetAdm()
        {
            return await db.Usuario.Where(w => w.DeletadoEm == null).Where(u => u.TipoUsuario.Titulo == "Administrador").Include(i => i.TipoUsuario).ToListAsync();
        }

        public async Task<Usuario> Post(Usuario usuario)
        {

            var funcionario = usuario.Email.ToLower().Contains("@thougthworks.com");

            if (funcionario)
            {

                try
                {
                    var tipoAdm = await db.TipoUsuario.FirstOrDefaultAsync(a => a.Titulo == "Funcionario");
                    usuario.TipoUsuarioId = tipoAdm.TipoUsuarioId;

                }
                catch (Exception)
                {
                    throw;
                }

            }
            else
            {

                try
                {
                    var tipoCom = await db.TipoUsuario.FirstOrDefaultAsync(a => a.Titulo == "Comunidade");
                    usuario.TipoUsuarioId = tipoCom.TipoUsuarioId;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            await db.Usuario.AddAsync(usuario);
            await db.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> Put(Usuario usuario)
        {
            try
            {
                db.Entry(usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task<Usuario> ExistEmail(string email)
        {
            try
            {
                return await db.Usuario.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Mensagem(string email, string senha)
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
                _mailMessage.Subject = "COMMUNITY IN LOUNGE CODE XP";
                _mailMessage.IsBodyHtml = true;
                _mailMessage.Body = "<b>Olá Tudo bem ??</b><p>Essa é a sua nova senha " + senha + "</p>";

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

    public class Header {
        public string Nome { get; set; }
        public string Foto { get; set; }
    }
}

