using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TesteUGB.Data;
using TesteUGB.Models;
using TesteUGB.Repositorio;
using TesteUGBMVC.Models;

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


        [HttpGet]
        public async Task<IActionResult> DeletarServico(int id)
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
                    var servicoModel = JsonConvert.DeserializeObject<ServicoViewModel>(content);
                    return View(servicoModel);
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

        [HttpPost]
        public async Task<IActionResult> EditarServico(ServicoViewModel servicoEditado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var servicoModel = new ServicoModel
                    {
                        Id = servicoEditado.Id,
                        NomeDoServico = servicoEditado.NomeDoServico,
                        DescricaoDoServico = servicoEditado.DescricaoDoServico,
                        PrazoEntregaPadrao = servicoEditado.PrazoEntregaPadrao
                    };

                    var servicoJson = JsonConvert.SerializeObject(servicoModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(servicoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{servicoModel.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Serviço editado com sucesso!";
                        return RedirectToAction("ListaServicos");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao editar o serviço na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao editar o serviço: " + ex.Message);
                }
            }

            return View(servicoEditado);
        }

        //----------------------------------------------------------------------
        [HttpGet]
        public IActionResult SolicitarServico()
        {
            // Aqui, você pode preparar os dados para exibir na View de solicitação de serviço.
            // Certifique-se de que sua View esteja configurada para exibir as informações corretas, como serviços e fornecedores.

            // Substitua a linha abaixo pela lógica real para obter serviços e fornecedores do banco de dados.
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
                // Crie uma nova solicitação com base nos dados fornecidos.
                var novaSolicitacao = new SolicitacaoServicoModel
                {
                    ServicoId = solicitacao.ServicoId,
                    FornecedorId = solicitacao.FornecedorId,
                    // Preencha outras propriedades da solicitação, se houver.
                };

                // Adicione a nova solicitação ao contexto do banco de dados.
                _context.SolicitacoesServico.Add(novaSolicitacao);

                // Salve as alterações no banco de dados.
                _context.SaveChanges();

                // Redirecione para a ação que lista as solicitações.
                return RedirectToAction("ListaServicosSolicitados", new { fornecedorId = novaSolicitacao.FornecedorId, servicoId = novaSolicitacao.ServicoId });
            }

            // Se a ModelState não for válida, retorne a View de SolicitarServiço com os erros.
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
                // Adicione outras propriedades aqui, se necessário.
            }).ToList();

            return View(viewModelList);
        }
    }

}
