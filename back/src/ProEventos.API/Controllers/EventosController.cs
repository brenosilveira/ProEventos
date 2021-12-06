using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;

namespace ProEventos.API.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class EventosController : ControllerBase
   {
      private readonly IEventoService eventoService;

      public EventosController(IEventoService eventoService)
      {
         this.eventoService = eventoService;

      }

      [HttpGet]
      public async Task<IActionResult> Get()
      {
         try
         {
               var eventos = await this.eventoService.GetAllEventosAsync(true);
               if (eventos == null) return NotFound("Nenhum evento foi encontrado.");

               return Ok(eventos);
         }
         catch (Exception ex)
         {
               return this.StatusCode(StatusCodes.Status500InternalServerError,
               $"Erro ao tentar recuperar um evento. Erro: {ex.Message}");
         }
      }

      [HttpGet("{id}")]
      public async Task<IActionResult> GetById(int id)
      {
         try
         {
               var evento = await this.eventoService.GetEventoByIdAsync(id, true);
               if (evento == null) return NotFound("Evento por Id não encontrados.");

               return Ok(evento);
         }
         catch (Exception ex)
         {
               return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Erro ao tentar recuperar um evento. Erro: {ex.Message}");
         }
      }

      [HttpGet("{tema}/tema")]
      public async Task<IActionResult> GetByTema(string tema)
      {
         try
         {
               var evento = await this.eventoService.GetAllEventosByTemaAsync(tema, true);
               if (evento == null) return NotFound("Evento por tema não encontrados.");

               return Ok(evento);
         }
         catch (Exception ex)
         {
               return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Erro ao tentar recuperar um evento. Erro: {ex.Message}");
         }
      }

      [HttpPost]
      public async Task<IActionResult> Post(Evento model)
      {
         try
         {
               var evento = await this.eventoService.addEventos(model);
               if (evento == null) return BadRequest("Erro ao tentar adicionar evento.");

               return Ok(evento);
         }
         catch (Exception ex)
         {
               return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Erro ao tentar adicionar um evento. Erro: {ex.Message}");
         }
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> Put(int id, Evento model)
      {
         try
         {
               var evento = await eventoService.UpdateEvento(id, model);
               if (evento == null) return BadRequest("Erro ao tentar adicionar evento.");

               return Ok(evento);
         }
         catch (Exception ex)
         {
               return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Erro ao tentar atualizar um evento. Erro: {ex.Message}");
         }
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         try
         {
               return await this.eventoService.DeleteEvento(id) ?
                  Ok("Deletado.") :
                  BadRequest("Evento não deletado");
         }
         catch (Exception ex)
         {
               return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Erro ao tentar deletar um evento. Erro: {ex.Message}");
         }
      }
   }
}