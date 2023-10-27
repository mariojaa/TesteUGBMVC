namespace TesteUGBMVC.Models
{
    public class FornecedorViewModel
    {
        public int Id { get; set; }
        public string NomeEmpresaFornecedora { get; set; }
        public string EnderecoFornecedor { get; set; }
        public int NumeroEnderecoFornecedor { get; set; }
        public string BairroEnderecoFornecedor { get; set; }
        public string CidadeEnderecoFornecedor { get; set; }
        public string EstadoEnderecoFornecedor { get; set; }
        public string PaisEnderecoFornecedor { get; set; }
        public string EmailFornecedor { get; set; } //Colocar Formato padrão de Email
        public long CNPJFornecedor { get; set; } //Colocar Formato padrão NPJ para cadastro
        public long InscricaoEstadualEMunicipalFornecedor { get; set; } //Colocar Formato padrão da inscrição
        public List<ServicoViewModel>? Servicios { get; set; }
    }
}
