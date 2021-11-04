using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Services
{
    public interface IMessaging<TObject>
    {
        public TObject Mensagem { get; set; }
    }
}
