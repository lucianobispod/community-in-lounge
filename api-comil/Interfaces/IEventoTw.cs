using System.Collections.Generic;
using System.Threading.Tasks;
using api_comil.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_comil.Interfaces
{
    public interface IEventoTw
    {
        Task<ActionResult<EventoTw>> Post(EventoTw evento);
        Task<ActionResult<EventoTw>> Get(int id);
        Task<ActionResult<List<EventoTw>>> GetPublic();
        Task<ActionResult<List<EventoTw>>> GetPrivate();
        Task<ActionResult<List<EventoTw>>> GetMounth(int mes);
        Task<ActionResult<EventoTw>> Put(EventoTw evento);
        Task<ActionResult<EventoTw>> Delete(EventoTw evento);
    }
}