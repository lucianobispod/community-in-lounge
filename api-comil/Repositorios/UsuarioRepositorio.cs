using System;
using System.Collections.Generic;
using System.Linq;
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
                return await db.Usuario.Where(w => w.DeletadoEm == null).Include(user => user.Comunidade).FirstOrDefaultAsync(f => f.UsuarioId == id);
        }

        public async Task<Usuario> Post(Usuario usuario)
        {
     
           var funcionario = usuario.Email.ToLower().Contains("@thougthworks.com");
           
           if (funcionario)
           {

                try
                {
                    var tipoAdm = await db.TipoUsuario.FirstOrDefaultAsync(a => a.Titulo == "Funcionario" );
                    usuario.TipoUsuarioId = tipoAdm.TipoUsuarioId;
                    
                }
                catch (Exception)
                {
                    throw;
                }

           }else{

               try
               {
                var tipoCom = await db.TipoUsuario.FirstOrDefaultAsync(a => a.Titulo == "Comunidade" );
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


    }
}

        