using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContratosMVVM.Auxiliares
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class TipoContato
    {
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
    }

    public class TiposContato
    {
        [JsonPropertyName("tipoContato")]
        public TipoContato TipoContato { get; set; }
    }

    public class Contato2
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("fantasia")]
        public string Fantasia { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("ie_rg")]
        public string IeRg { get; set; }

        [JsonPropertyName("endereco")]
        public string Endereco { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("fone")]
        public string Fone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        [JsonPropertyName("contribuinte")]
        public string Contribuinte { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

        [JsonPropertyName("celular")]
        public string Celular { get; set; }

        [JsonPropertyName("dataAlteracao")]
        public string DataAlteracao { get; set; }

        [JsonPropertyName("dataInclusao")]
        public string DataInclusao { get; set; }

        [JsonPropertyName("sexo")]
        public string Sexo { get; set; }

        [JsonPropertyName("clienteDesde")]
        public string ClienteDesde { get; set; }

        [JsonPropertyName("limiteCredito")]
        public string LimiteCredito { get; set; }

        [JsonPropertyName("dataNascimento")]
        public string DataNascimento { get; set; }

        [JsonPropertyName("tiposContato")]
        public List<TiposContato> TiposContato { get; set; }
    }

    public class ContatoNode
    {
        [JsonPropertyName("contato")]
        public Contato2 Contato { get; set; }
    }

    public class RetornoCliente
    {
        [JsonPropertyName("contatos")]
        public List<ContatoNode> Contatos { get; set; }
        [JsonPropertyName("erros")]
        public List<Erro> Erros { get; set; }
    }

    public class RootContato
    {
        [JsonPropertyName("retorno")]
        public RetornoCliente Retorno { get; set; }
    }

    public class Erro
    {
        [JsonPropertyName("cod")]
        public int Cod { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("erro")]
        public Erro erro { get; set; }
    }

}