using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;
using TesteUGB.Models;

namespace TesteUGBMVC.Models
{
    public class ServicoViewModel
    {
        public int Id { get; set; }
        public string NomeDoServico { get; set; }
        public string DescricaoDoServico { get; set; }
        public DateTime PrazoEntregaPadrao { get; set; } //Prazo Padrão para o tempo de serviço ser entregue (até x dias)
        public string TipoServico { get; set; }
        [ForeignKey("FornecedorviewModel")]
        public int FornecedorId { get; set; }

        [InverseProperty("Servicos")]
        public FornecedorViewModel Fornecedor { get; set; }
    }
}
