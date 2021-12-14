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
    public class ContatoDataService
    {

        private readonly CobrancaDbContext _context;
        private int stageCounter;

        public ContatoDataService(CobrancaDbContext ambiStoreDbContextFactory)
        {
            _context = ambiStoreDbContextFactory;

        }

        public async Task<CLIENTE> GetAsNoTracking(string cnpjcpf)
        {
            return await _context.Clientes.AsNoTracking()
               .FirstOrDefaultAsync(c => c.CNPJCPF == cnpjcpf);
        }

        public async Task<CLIENTE> AddOrUpdate(CLIENTE entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<CLIENTE>> GetAllAsNoTracking()
        {
            return await _context.Clientes.AsNoTracking()
               .ToListAsync();

        }

        public async Task<int> AddToCreateStage(CLIENTE entity)
        {
            stageCounter++;
            await _context.Clientes.AddAsync(entity);
            return stageCounter;
        }

        public async Task<int> AddToUpdateStage(int id, CLIENTE entity)
        {
            stageCounter++;

            _context.Clientes.Update(entity);
            return stageCounter;
        }

        public async Task SaveStage()
        {
            await _context.SaveChangesAsync();
            stageCounter = 0;
        }
    }
}
