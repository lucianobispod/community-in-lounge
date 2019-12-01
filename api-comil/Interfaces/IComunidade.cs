using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface IComunidade
    {
        Task<List<Comunidade>> Get();

        Task<Comunidade> Get(int id);

        Task<Comunidade> Post(Comunidade comunidade);

        Task<Comunidade> Put(Comunidade comunidade);

        Task<ActionResult<Comunidade>> Delete(Comunidade comunidade);
    }
}