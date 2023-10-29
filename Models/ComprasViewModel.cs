using TesteUGB.Models.Enum;

namespace TesteUGBMVC.Models
{
    public class ComprasViewModel
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public int CodigoDaSolicitacao { get; set; }
        public string Fabricante { get; set; }
        public int QuantidadeSolicitada { get; set; }
        public string DepartamentoSolicitante { get; set; }
        public DateTime DataSolicitada { get; set; }
        public DateTime DataPrevisaoEntregaProduto { get; set; }
        public TipoDoProdutoEnum TipoDoProduto { get; set; }
        public int ValorUnitarioDoProduto { get; set; }
        public int ValorTotal { get; set; } // Soma do valor unitario + quantidade
        public long NumeroNotaFiscalProduto { get; set; }
        public long CodigoEAN { get; set; } // Código de Barras do Produto
    }
}
