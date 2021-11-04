using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContratosMVVM.Domain
{
    public class CLIENTE : EntityBase
    {
        public string CNPJCPF { get; set; }
        public string RazãoSocial { get; set; }
        public string Telefone { get; set; }
        public string Endereço { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Email { get; set; }
        public string Representante { get; set; }
        public string CPFDoRepresentante { get; set; }
        public int DataMelhorVencimento { get; set; }
        public OBSERVACAO Observacao { get; set; }
        public int IDFirebird { get; set; }

        public List<CONTRATO> Contratos { get; set; }
        public bool HasContrato => Contratos?.Any(x => x.ContratoPDF is not null) ?? false;
    }
}
