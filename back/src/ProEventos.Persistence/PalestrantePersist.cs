using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Contrato;

namespace ProEventos.Persistence
{
    public class PalestrantePersistence : IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersistence(ProEventosContext context)
        {
            _context = context;
            //this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);

            if(includeEventos)
            {
                query = query
                        .Include(palestrante => palestrante.PalestrantesEventos)
                        .ThenInclude(palestranteEvento => palestranteEvento.Evento);

            }

            query = query.AsNoTracking().OrderBy(palestrante => palestrante.Id);

            return await query.AsSplitQuery().ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);

            if(includeEventos)
            {
                query = query
                        .Include(palestrante => palestrante.PalestrantesEventos)
                        .ThenInclude(palestranteEvento => palestranteEvento.Evento);

            }

            query = query.AsNoTracking()
                    .OrderBy(palestrante => palestrante.Id)
                    .Where(palestrante => palestrante.Nome.ToLower().Contains(nome.ToLower()));    

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);

            if(includeEventos)
            {
                query = query
                        .Include(palestrante => palestrante.PalestrantesEventos)
                        .ThenInclude(palestranteEvento => palestranteEvento.Evento);

            }

            query = query.AsNoTracking()
                    .OrderBy(palestrante => palestrante.Id)
                    .Where(palestrante => palestrante.Id == palestrante.Id);    

            return await query.FirstOrDefaultAsync();
        }
    }
}