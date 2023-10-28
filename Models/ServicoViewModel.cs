using MessagePack;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using TesteUGB.Models;

namespace TesteUGBMVC.Models
{
    public class ServicoViewModel
    {
        public int Id { get; set; }
        public string NomeDoServico { get; set; }
        public string DescricaoDoServico { get; set; }
        public DateTime PrazoEntregaPadrao { get; set; } //Prazo Padrão para o tempo de serviço ser entregue (até x dias)
        [Display(Name = "Fornecedor")]
        public int FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public FornecedorViewModel Fornecedor { get; set; }

        [Display(Name = "Fornecedores")]
        public List<SelectListItem> Fornecedores { get; set; }
    }
}
