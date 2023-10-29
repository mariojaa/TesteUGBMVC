using System;
using System.ComponentModel.DataAnnotations;
using TesteUGBMVC.Models.Enum;

namespace TesteUGBMVC.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome do Produto é obrigatório.")]
        public string NomeProduto { get; set; }

        public int NumeroPedidoProduto { get; set; }
        public string FornecedorProduto { get; set; }

        [Display(Name = "Quantidade em Estoque")]
        public int QuantidadeTotalEmEstoque { get; set; }

        [Display(Name = "Quantidade Mínima em Estoque")]
        public int QuantidadeMinimaEmEstoque { get; set; }

        [Display(Name = "Setor de Depósito")]
        public string SetorDeDeposito { get; set; }

        [Display(Name = "Data de Cadastro")]
        [DataType(DataType.Date)]
        public DateTime DataCadastroProduto { get; set; }

        [Display(Name = "Data de Previsão de Entrega")]
        [DataType(DataType.Date)]
        public DateTime DataPrevisaoEntregaProduto { get; set; }

        [Display(Name = "Tipo do Produto (Unitário ou Pacote)")]
        public TipoDoProdutoEnum TipoDoProdutoUnitarioOuPacote { get; set; }

        [Display(Name = "Valor Unitário do Produto")]
        [DataType(DataType.Currency)]
        public decimal ValorUnitarioDoProduto { get; set; }

        [Display(Name = "Número da Nota Fiscal do Produto")]
        public long NumeroNotaFiscalProduto { get; set; }

        [Display(Name = "Código de Barras do Produto")]
        public long CodigoEAN { get; set; }

    }
}
