using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteUGB.Data;
using TesteUGB.Models;
using TesteUGB.Repositorio;
using TesteUGBMVC.Models;
using TesteUGBMVC.ViewModels;

namespace TesteUGBMVC.Controllers
{
    public class ServicoController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/servico";
        private readonly HttpClient httpClient;
        private readonly ServicoRepository _servicoRepository;
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly TesteUGBDbContext _context;

        public ServicoController(ServicoRepository servicoRepository, FornecedorRepository fornecedorRepository, TesteUGBDbContext context)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
            _servicoRepository = servicoRepository;
            _fornecedorRepository = fornecedorRepository;
            _context = context;
        }

        public async Task<IActionResult> ListaServicos()
        {
            List<ServicoViewModel> servicos = null;

            var servicosModel = await _servicoRepository.Buscar();

            if (servicosModel != null)
            {
                servicos = servicosModel.Select(s => new ServicoViewModel
                {
                    Id = s.Id,
                    NomeDoServico = s.NomeDoServico,
                    DescricaoDoServico = s.DescricaoDoServico,
                    PrazoEntregaPadrao = s.PrazoEntregaPadrao,
                    NomeEmpresaFornecedora = s.Fornecedor != null ? s.Fornecedor.NomeEmpresaFornecedora : "N/A"
                }).ToList();

                return View(servicos);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao obter a lista de serviços.");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CriarServico()
        {

            var fornecedores = await _fornecedorRepository.BuscarFornecedores();

            var fornecedoresSelectList = fornecedores.Select(f => new SelectListItem //mapeia lista fornecedor
            {
                Text = f.NomeEmpresaFornecedora,
                Value = f.Id.ToString()
            }).ToList();

            // popula combobox
            var novoServico = new ServicoViewModel
            {
                Fornecedores = fornecedoresSelectList
            };

            return View(novoServico);
        }


        [HttpPost]
        public async Task<IActionResult> CriarServico(ServicoViewModel novoServico)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var fornecedorSelecionado = await _fornecedorRepository.FindById(novoServico.FornecedorId);

                    if (fornecedorSelecionado != null)
                    {
                        novoServico.NomeEmpresaFornecedora = fornecedorSelecionado.NomeEmpresaFornecedora;

                        var servicoModel = new ServicoModel
                        {
                            NomeDoServico = novoServico.NomeDoServico,
                            DescricaoDoServico = novoServico.DescricaoDoServico,
                            PrazoEntregaPadrao = novoServico.PrazoEntregaPadrao,
                            FornecedorId = fornecedorSelecionado.Id,
                            NomeEmpresaFornecedora = fornecedorSelecionado.NomeEmpresaFornecedora
                        };

                        _context.Add(servicoModel);
                        await _context.SaveChangesAsync();

                        TempData["MensagemSucesso"] = "Serviço criado com sucesso!";
                        return RedirectToAction("ListaServicos");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Fornecedor não encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao criar o serviço: " + ex.Message);
                }
            }

            var fornecedores = await _fornecedorRepository.BuscarFornecedores();
            novoServico.Fornecedores = fornecedores.Select(f => new SelectListItem
            {
                Text = f.NomeEmpresaFornecedora,
                Value = f.Id.ToString()
            }).ToList();

            return View(novoServico);
        }

        [HttpGet("DeletarServico/{id}")]
        public async Task<IActionResult> DeletarServicoSolicitado(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Serviço excluído com sucesso!";
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao excluir o serviço na API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir o serviço: " + ex.Message);
            }

            return RedirectToAction("ListaServicos");
        }

        [HttpGet]
        public async Task<IActionResult> EditarServico(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var produtoModel = JsonConvert.DeserializeObject<ServicoViewModel>(content);
                    return View(produtoModel);
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao obter o serviço da API.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao obter o serviço: " + ex.Message);
                return View();
            }
        }


        [HttpGet]
        public IActionResult SolicitarServico()
        {
            var servicos = _context.Servicos.ToList();
            var fornecedores = _context.Fornecedores.ToList();

            var servicoViewModel = new ServicoViewModel
            {
                Servicos = servicos.Select(s => new SelectListItem
                {
                    Text = s.NomeDoServico,
                    Value = s.Id.ToString()
                }).ToList(),
                Fornecedores = fornecedores.Select(f => new SelectListItem
                {
                    Text = f.NomeEmpresaFornecedora,
                    Value = f.Id.ToString()
                }).ToList()
            };

            return View(servicoViewModel);
        }

        [HttpPost]
        public IActionResult SolicitarServico(ServicoViewModel solicitacao)
        {
            if (!ModelState.IsValid)
            {
                var novaSolicitacao = new SolicitacaoServicoModel
                {
                    ServicoId = solicitacao.ServicoId,
                    FornecedorId = solicitacao.FornecedorId,
                };

                _context.SolicitacoesServico.Add(novaSolicitacao);
                _context.SaveChanges();

                return RedirectToAction("ListaServicosSolicitados", new { fornecedorId = novaSolicitacao.FornecedorId, servicoId = novaSolicitacao.ServicoId });
            }

            var servicos = _context.Servicos.ToList();
            var fornecedores = _context.Fornecedores.ToList();

            solicitacao.Servicos = servicos.Select(s => new SelectListItem
            {
                Text = s.NomeDoServico,
                Value = s.Id.ToString()
            }).ToList();

            solicitacao.Fornecedores = fornecedores.Select(f => new SelectListItem
            {
                Text = f.NomeEmpresaFornecedora,
                Value = f.Id.ToString()
            }).ToList();

            return View(solicitacao);
        }
        public IActionResult ListaServicosSolicitados(int fornecedorId, int servicoId)
        {
            var servicosSolicitados = _context.SolicitacoesServico
                .Include(s => s.Servico)
                .Include(s => s.Fornecedor)
                .ToList();

            var viewModelList = servicosSolicitados.Select(s => new SolicitacaoServicoViewModel
            {
                Servico = s.Servico,
                Fornecedor = s.Fornecedor,
            }).ToList();

            return View(viewModelList);
        }
    }

}
