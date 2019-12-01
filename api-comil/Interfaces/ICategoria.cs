using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface ICategoria
    {
        Task<List<Categoria>> Get();
        Task<Categoria> Get(int id);
        Task<Categoria> Post(Categoria categoria);
        Task<ActionResult<Categoria>> Delete(Categoria categoria);
        Task<Categoria> Get(Categoria categoria);

    }
}