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
    public class ContratoBaseDataService : IGenericDataService<CONTRATO_BASE>
    {
        private readonly CobrancaDbContext _cobrancaDbContext;

        public ContratoBaseDataService(CobrancaDbContext cobrancaDbContext)
        {
            _cobrancaDbContext = cobrancaDbContext;
        }


        public async Task<CONTRATO_BASE> Create(CONTRATO_BASE entity)
        {
            EntityEntry<CONTRATO_BASE> createdResult = await _cobrancaDbContext.Set<CONTRATO_BASE>().AddAsync(entity);
            await _cobrancaDbContext.SaveChangesAsync();

            return createdResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            var toRemove = _cobrancaDbContext.ContratoBases.FirstOrDefaultAsync(x => x.Id == id);
            if (toRemove is not null)
            {
                _cobrancaDbContext.Remove(toRemove);
                await _cobrancaDbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<CONTRATO_BASE> Get(int id)
        {
            return await _cobrancaDbContext.ContratoBases.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CONTRATO_BASE> GetAsNoTracking(int id)
        {
            return await _cobrancaDbContext.ContratoBases.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<ICollection<CONTRATO_BASE>> GetAll()
        {
            return await _cobrancaDbContext.ContratoBases.ToListAsync();

        }

        public async Task<ICollection<CONTRATO_BASE>> GetAllAsNoTracking()
        {
            return await _cobrancaDbContext.ContratoBases.AsNoTracking().OrderBy(x=>x.Nome).ToListAsync();

        }

        public async Task<CONTRATO_BASE> Update(int id, CONTRATO_BASE entity)
        {

            entity.Id = id;
            var tracked = _cobrancaDbContext.ChangeTracker.Entries<CONTRATO_BASE>().FirstOrDefault(x => x.Entity.Id == id);
            if (tracked is not null)
            {
                tracked.State = EntityState.Detached;
            }
            
            _cobrancaDbContext.ContratoBases.Update(entity);


            await _cobrancaDbContext.SaveChangesAsync();

            return entity;
        }
    }
}
