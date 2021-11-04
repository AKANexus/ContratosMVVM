using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Domain
{
    public class CONTRATO_BASE : EntityBase
    {


        public List<CONTRATO> Contratos { get; set; }
        public SETOR Setor { get; set; }
        public int SetorId { get; set; }

        public string Nome { get; set; }

        public string Descrição { get; set; }
    }
}
