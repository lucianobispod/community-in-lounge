using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_comil.Interfaces;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Repositorios
{
    public class ComunidadeRepositorio : IComunidade
    {
        communityInLoungeContext db = new communityInLoungeContext();


        public async Task<ActionResult<Comunidade>> Delete(Comunidade comunidade)
        {
            await db.SaveChangesAsync();
            return comunidade;
        }

        public async Task<List<Comunidade>> GetByUser(int id)
        {
            List<Comunidade> ListaDeComunidade = await db.Comunidade
                                                         .Where(w => w.DeletadoEm == null)
                                                         .Where(w => w.ResponsavelUsuarioId == id)
                                                         .ToListAsync();
            return ListaDeComunidade;
        }

        public async Task<Comunidade> Get(int id)
        {
            return await db.Comunidade.Where(w => w.DeletadoEm == null).SingleOrDefaultAsync(a => a.ComunidadeId == id);
        }

        public async Task<Comunidade> Get(string nome)
        {
            return await db.Comunidade.Where(w => w.DeletadoEm == null).SingleOrDefaultAsync(a => a.Nome == nome);
        }
        public async Task<Comunidade> Post(Comunidade comunidade)
        {
            await db.Comunidade.AddAsync(comunidade);
            await db.SaveChangesAsync();

            return comunidade;
        }

        public async Task<Comunidade> Put(Comunidade comunidade)
        {
            db.Entry(comunidade).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return comunidade;
        }
    }
}
