using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface IEvento
    {
        Task<ActionResult<Evento>> Post(Evento evento);                                                                         // feito
        Task<ActionResult<Evento>> Update(Evento evento);//validações de edição                                                               feito
        Task<ActionResult> Delete();                                                                                            // feito
        Task<ActionResult<List<Evento>>> Get();                                                                                 // feito
        Task<Evento> Get(int id);                                                                                               // feito
        Task<ActionResult<List<ResponsavelEventoTw>>> PendingUser(int id); //eventos pendentes do usuario                       //feito
        Task<ActionResult<List<ResponsavelEventoTw>>> ApprovedUser(int id); //eventos aprovados do usuario                      //feito
        Task<ActionResult<List<ResponsavelEventoTw>>> RealizeUser(int id); //eventos em analise do usuario                      //feito
        Task<ActionResult<List<ResponsavelEventoTw>>> PendingMounth(int id); //eventos pendentes para o adm aceitar ou recusar  //feito
        Task<Evento> Reject(Evento evento, int responsavel); //eventos pendentes para o adm aceitar ou recusar                  feito
        Task<Evento> Accept(Evento evento, int responsavel); //eventos pendentes para o adm aceitar ou recusar                  feito
        Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsReject(int id); // eventos que o adm recusou                       feito
        Task<ActionResult<List<ResponsavelEventoTw>>> MyEventsAccept(int id); // eventos que o adm aceitou                       feito
        Task<ActionResult<List<Evento>>> Mounth(); //filtro por mes                                                             feito
        Task<ActionResult<List<Evento>>> EventByCategory(int id);                                                               //feito      

    }
}