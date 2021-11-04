using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Domain;

namespace ContratosMVVM.Services
{
    public interface IGenericDataService<T> where T : EntityBase
    {
        public Task<T> Create(T entity);

        public Task<bool> Delete(int id);

        public Task<T> Get(int id);

        public Task<T> GetAsNoTracking(int id);
        
        public Task<ICollection<T>> GetAll();

        public Task<ICollection<T>> GetAllAsNoTracking();

        public Task<T> Update(int id, T entity);
    }
}
