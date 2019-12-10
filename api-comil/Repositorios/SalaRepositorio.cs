using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Repositorios
{
    public class SalaRepositorio
    {
          communityInLoungeContext db = new communityInLoungeContext();


        public async Task<List<Sala>> Get()
        {
            return await db.Sala.Where(w => w.DeletadoEm == null).ToListAsync();
        }


        public async Task<Sala> Get(int id)
        {
            return await db.Sala.FindAsync(id);
        }
    }
}