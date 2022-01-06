namespace ContratosMVVM.Auxiliares
{
    public class ProgressReport
    {
        public ProgressReport(string cAIXA, string tABELA, string eNTRADA)
        {
            CAIXA = cAIXA;
            TABELA = tABELA;
            ENTRADA = eNTRADA;
        }

        public string CAIXA { get; set; }
        public string TABELA { get; set; }
        public string ENTRADA { get; set; }
    }
}