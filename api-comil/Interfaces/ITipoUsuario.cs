using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface ITipoUsuario
    {
          Task<List<TipoUsuario>> Get();

         Task<TipoUsuario> Get(int id);

      

         
         


    }
}