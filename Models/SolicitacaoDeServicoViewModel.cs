using TesteUGB.Models;

public class SolicitacaoServicoViewModel
{
    public int Id { get; set; }
    public int ServicoId { get; set; }
    public int FornecedorId { get; set; }
    public string NomeDoServico { get; set; }
    public string NomeEmpresaFornecedora { get; set; } // Adicione esta propriedade
    public DateTime Data { get; set; }
    public ServicoModel Servico { get; set; }
    public FornecedorModel Fornecedor { get; set; }
}
