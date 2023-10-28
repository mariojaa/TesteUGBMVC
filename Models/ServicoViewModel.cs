using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace TesteUGBMVC.Models
{
    public class ServicoViewModel
    {
        public ServicoViewModel()
        {
            ServicosSolicitados = new List<ServicoViewModel>();
        }
        public int Id { get; set; }
        public string NomeDoServico { get; set; }
        public string DescricaoDoServico { get; set; }
        public DateTime PrazoEntregaPadrao { get; set; }
        public int FornecedorId { get; set; } // ID do fornecedor selecionado
        public string NomeEmpresaFornecedora { get; set; } // Nome da empresa fornecedora
        public List<SelectListItem> Fornecedores { get; set; } // Lista de fornecedores para a ComboBox
        public int ServicoId { get; set; }
        public List<SelectListItem> Servicos { get; set; } // Lista de serviços para a ComboBox
        public List<ServicoViewModel> ServicosSolicitados { get; set; } // Lista de solicitações de serviço

    }
}
