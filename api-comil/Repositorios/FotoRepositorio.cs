using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.EntityFrameworkCore;

namespace api_comil.Repositorios {
    public class FotoRepositorio {

        public async Task<Usuario> Alterar (Microsoft.AspNetCore.Http.IFormFile arquivo, Usuario usuario) {
            using (communityInLoungeContext _contexto = new communityInLoungeContext ()) {
                _contexto.Entry (usuario).State = EntityState.Modified;
                await _contexto.SaveChangesAsync ();
            }
            return usuario;

        }

        public async Task<Usuario> BuscarPorID (int id) {
            using (communityInLoungeContext _contexto = new communityInLoungeContext ()) {
                return await _contexto.Usuario.FindAsync (id);
            }
        }
        
        public async Task<Usuario> Salvar (Usuario usuario) {
            using (communityInLoungeContext _contexto = new communityInLoungeContext ()) {
                await _contexto.AddAsync (usuario);
                await _contexto.SaveChangesAsync ();
            }

            return usuario;
        }
    }
}