using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_comil.Interfaces;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Repositorios {

    public class TipoUsuarioRepositorio : ITipoUsuario
    {
        communityInLoungeContext db = new communityInLoungeContext();

        public async Task<TipoUsuario> Get(string titulo)
        {
          return await db.TipoUsuario.SingleOrDefaultAsync(n => n.Titulo == titulo);
        }

        public async Task<TipoUsuario> Get(int id)
        {
            return await db.TipoUsuario.FindAsync(id);
        }

        public async Task<List<TipoUsuario>> Get()
        {
           return await db.TipoUsuario.ToListAsync();
        }
    }
}