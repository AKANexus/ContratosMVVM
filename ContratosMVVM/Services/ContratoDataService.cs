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
    public class ContratoDataService : IGenericDataService<CONTRATO>
    {
        private readonly CobrancaDbContext _cobrancaDbContext;

        public ContratoDataService(CobrancaDbContext cobrancaDbContext)
        {
            _cobrancaDbContext = cobrancaDbContext;
        }


        public async Task<CONTRATO> Create(CONTRATO entity)
        {
            EntityEntry<CONTRATO> createdResult = await _cobrancaDbContext.Set<CONTRATO>().AddAsync(entity);
            await _cobrancaDbContext.SaveChangesAsync();

            return createdResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            var toRemove = await _cobrancaDbContext.Contratos.FirstOrDefaultAsync(x => x.Id == id);
            if (toRemove is not null)
            {
                _cobrancaDbContext.Remove(toRemove);
                await _cobrancaDbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<CONTRATO> Get(int id)
        {
            return await _cobrancaDbContext.Contratos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CONTRATO> GetAsNoTracking(int id)
        {
            return await _cobrancaDbContext.Contratos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<List<CONTRATO>> GetAllAsNoTrackingByCliente(CLIENTE cliente)
        {
            return await _cobrancaDbContext.Contratos.AsNoTracking().Where(x => x.FirebirdIDCliente == cliente.IDFirebird).ToListAsync();


        }

        public async Task<ICollection<CONTRATO>> GetAll()
        {
            return await _cobrancaDbContext.Contratos.ToListAsync();

        }

        public async Task<ICollection<CONTRATO>> GetAllAsNoTracking()
        {
            return await _cobrancaDbContext.Contratos.Include(x=>x.ContratoBase).AsNoTracking().ToListAsync();

        }

        public async Task<CONTRATO> Update(int id, CONTRATO entity)
        {

            entity.Id = id;
            foreach (var entry in _cobrancaDbContext.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
            //var tracked = _cobrancaDbContext.ChangeTracker.Entries<CONTRATO>().FirstOrDefault(x => x.Entity.Id == id);
            //if (tracked is not null)
            //{
            //    tracked.State = EntityState.Detached;
            //}
            _cobrancaDbContext.Contratos.Update(entity);
            await _cobrancaDbContext.SaveChangesAsync();

            return entity;
        }
    }

}
