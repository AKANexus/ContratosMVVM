using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Context;
using ContratosMVVM.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContratosMVVM.Services
{
    public class ObservacaoDataService
    {
        private readonly CobrancaDbContext _context;

        public ObservacaoDataService(CobrancaDbContext context)
        {
            _context = context;
        }
        public async Task<OBSERVACAO> Create(OBSERVACAO entity)
        {
            EntityEntry<OBSERVACAO> createdResult = await _context.Set<OBSERVACAO>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return createdResult.Entity;
        }

        public async Task<OBSERVACAO> GetAsNoTrackingByFirebirdId(int firebirdId)
        {
            return await _context.Observacoes.Where(x => x.FirebirdId == firebirdId).FirstOrDefaultAsync();
        }


        public async Task<OBSERVACAO> Update(int id, OBSERVACAO entity)
        {
            entity.Id = id;
            var tracked = _context.ChangeTracker.Entries<OBSERVACAO>().FirstOrDefault(x => x.Entity.Id == id);
            if (tracked is not null)
            {
                tracked.State = EntityState.Detached;
            }

            _context.Observacoes.Update(entity);


            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
