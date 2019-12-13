using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface IEvento
    {
        Task<ActionResult<Evento>> Post(Evento evento);                                                                         
        Task<ActionResult<Evento>> Update(Evento evento);//validações de edição                                                              
        Task<Evento> Reject(Evento evento, int responsavel);   
        Task<Evento> Accept(Evento evento, int responsavel);   
       Task<ActionResult<Evento>> Delete(Evento evento);   
        

        Task<ActionResult<List<Evento>>> Get();  
        Task<Evento> Get(int id);                                                                                               
         Task<ActionResult<List<Evento>>> PendingMounth(int mes); 
        

        Task<ActionResult<List<Evento>>> PendingUser(int id); //eventos pendentes do usuario                       
        Task<ActionResult<List<Evento>>> ApprovedUser(int id); //eventos aprovados do usuario                     
        Task<ActionResult<List<Evento>>> RealizeUser(int id); //eventos em realizados do usuario                      


        // Task<ActionResult<List<Evento>>> MyEventsReject(int id); // eventos que o adm recusou                      
        Task<ActionResult<List<Evento>>> MyEventsAccept(int id); // eventos que o adm aceitou                            
    }
}