using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface IUsuario
    {
        Task<Usuario> Post(Usuario usuario);
        Task<Usuario> Get(int id);
        Task<Usuario> Put(Usuario usuario);
        Task<Usuario> Delete(Usuario usuario);
    }
}