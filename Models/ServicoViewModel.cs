using MessagePack;
using TesteUGB.Models;

namespace TesteUGBMVC.Models
{
    public class ServicoViewModel
    {
        public int Id { get; set; }
        public string NomeDoServico { get; set; }
        public string DescricaoDoServico { get; set; }
        public DateTime PrazoEntregaPadrao { get; set; } //Prazo Padrão para o tempo de serviço ser entregue (até x dias)
        public List<FornecedorModel> Fornecedores { get; set; }
        public int FornecedorSelecionadoId { get; set; }
        public List<ServicoModel> Servicos { get; set; }
        public string TipoServico { get; set; }
    }
}
