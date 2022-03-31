using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Domain
{
    public class CONTRATO : EntityBase
    {
        public int FirebirdIDCliente { get; set; }
        public decimal ValorUnitário { get; set; }
        public decimal Quantidade { get; set; }
        public CONTRATO_BASE ContratoBase { get; set; }
        public int ContratoBaseId { get; set; }
        public int Vigência { get; set; }
        public string ContratoPDF { get; set; }
        //public CLIENTE Cliente { get; set; }
        [NotMapped] public decimal ValorTotalDoContrato => (ValorUnitário * Quantidade);

        public string Observacao { get; set; }
        //ALTER TABLE Contratos ADD ContratoPDF TEXT;

    }
}
