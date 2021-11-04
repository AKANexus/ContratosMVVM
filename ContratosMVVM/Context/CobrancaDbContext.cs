using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContratosMVVM.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContratosMVVM.Context
{
    public class CobrancaDbContext : DbContext
    {
        public CobrancaDbContext(DbContextOptions options) : base(options)
        {

        }

        public CobrancaDbContext() : base()
        {

        }

        public DbSet<CONTRATO_BASE> ContratoBases { get; set; }
        public DbSet<SETOR> Setors { get; set; }
        public DbSet<CONTRATO> Contratos { get; set; }
        public DbSet<OBSERVACAO> Observacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //Debugger.Launch();

            mb.Entity<SETOR>()
                .HasMany(x => x.ContratoBases)
                .WithOne(x => x.Setor)
                .HasForeignKey(x => x.SetorId);

            mb.Entity<CONTRATO_BASE>()
                .HasMany(x => x.Contratos)
                .WithOne(x => x.ContratoBase)
                .HasForeignKey(x => x.ContratoBaseId)
                .IsRequired(false);
            
            base.OnModelCreating(mb);

        }
    }
}