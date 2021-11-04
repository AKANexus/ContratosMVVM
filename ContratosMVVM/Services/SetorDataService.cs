using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Context;
using ContratosMVVM.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContratosMVVM.Services
{
    public class SetorDataService : IGenericDataService<SETOR>
    {
        private readonly CobrancaDbContext _cobrancaDbContext;

        public SetorDataService(CobrancaDbContext cobrancaDbContext)
        {
            _cobrancaDbContext = cobrancaDbContext;
        }


        public async Task<SETOR> Create(SETOR entity)
        {
            var entry = await _cobrancaDbContext.Setors.AddAsync(entity);
            return entry.Entity;
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

        public async Task<SETOR> Get(int id)
        {
            return await _cobrancaDbContext.Setors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SETOR> GetAsNoTracking(int id)
        {
            return await _cobrancaDbContext.Setors.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<ICollection<SETOR>> GetAll()
        {
            return await _cobrancaDbContext.Setors.ToListAsync();

        }

        public async Task<ICollection<SETOR>> GetAllAsNoTracking()
        {
            return await _cobrancaDbContext.Setors.AsNoTracking().ToListAsync();

        }

        public async Task<SETOR> Update(int id, SETOR entity)
        {

            entity.Id = id;

            _cobrancaDbContext.Setors.Update(entity);
            await _cobrancaDbContext.SaveChangesAsync();

            return entity;
        }
    }
}
