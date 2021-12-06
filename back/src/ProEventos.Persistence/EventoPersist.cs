using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Persistence
{
    public class EventoPersistence : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventoPersistence(ProEventosContext context)
        {
            _context = context;
            //this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                        .Include(evento => evento.PalestrantesEventos)
                        .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);

            }

            query = query.AsNoTracking().OrderBy(evento => evento.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                        .Include(evento => evento.PalestrantesEventos)
                        .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
            }

            query = query.AsNoTracking()
                    .OrderBy(evento => evento.Id)
                    .Where(evento => evento.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                        .Include(e => e.PalestrantesEventos)
                        .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking()
                    .OrderBy(e => e.Id)
                    .Where(e => e.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}