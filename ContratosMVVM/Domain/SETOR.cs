using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Domain
{
    public class SETOR : EntityBase
    {

        public string Setor { get; set; }
        public List<CONTRATO_BASE> ContratoBases { get; set; }
    }
}
