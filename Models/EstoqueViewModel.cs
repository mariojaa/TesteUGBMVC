using TesteUGB.Models.Enum;

namespace TesteUGBMVC.ViewModels
{
    public class EstoqueViewModel
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public int QuantidadeTotalEmEstoque { get; set; }
        public int QuantidadeMinimaEmEstoque { get; set; }
        public string SetorDeDeposito { get; set; }
        public DateTime DataCadastroProduto { get; set; }
        public TipoDoProdutoEnum TipoDoProdutoUnitarioOuPacote { get; set; }
        public long NumeroNotaFiscalProduto { get; set; }
        public long CodigoEAN { get; set; }
        public int QuantidadeAtualEmEstoque { get; set; }
        public int Quantidade { get; set; }

    }
}
