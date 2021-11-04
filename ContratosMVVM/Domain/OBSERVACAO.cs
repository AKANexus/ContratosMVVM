using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Domain
{
    public class OBSERVACAO : EntityBase
    {
        public string Texto { get; set; }

        public int FirebirdId { get; set; }
    }
}
