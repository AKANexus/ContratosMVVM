using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContratosMVVM.Context
{
    public class CobrancaDbContextFactory/* : IDesignTimeDbContextFactory<AmbiStoreDbContext>*/
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public CobrancaDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }



        public CobrancaDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<CobrancaDbContext> options = new DbContextOptionsBuilder<CobrancaDbContext>();
            _configureDbContext(options);
            return new CobrancaDbContext(options.Options);
        }
    }
}
