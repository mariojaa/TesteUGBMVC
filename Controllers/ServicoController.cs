using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

        public ServicoController(ServicoRepository servicoRepository, FornecedorRepository fornecedorRepository)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
            _servicoRepository = servicoRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<IActionResult> ListaServicos()
        {
            List<ServicoViewModel> servicos = null;
            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                servicos = JsonConvert.DeserializeObject<List<ServicoViewModel>>(content);
                return View(servicos);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao processar a solicitação.");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CriarServico()
        {
            // Carregue a lista de fornecedores da sua fonte de dados (banco de dados, serviço, etc.)
            var fornecedores = await _fornecedorRepository.BuscarFornecedores(); // Substitua pelo método real que carrega os fornecedores

            // Mapeie a lista de fornecedores para SelectListItems
            var fornecedoresSelectList = fornecedores.Select(f => new SelectListItem
            {
                Text = f.NomeEmpresaFornecedora,
                Value = f.Id.ToString()
            }).ToList();

            // Preencha o campo Fornecedores no modelo
            var novoServico = new ServicoViewModel
            {
                Fornecedores = fornecedoresSelectList
            };

            return View(novoServico);
        }


        [HttpPost]
        public async Task<IActionResult> CriarServico(ServicoViewModel novoServico)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtenha o fornecedor selecionado
                    var fornecedorSelecionado = await _fornecedorRepository.FindById(novoServico.FornecedorId); // Substitua pelo método real que obtém o fornecedor pelo ID

                    if (fornecedorSelecionado != null)
                    {
                        var servicoModel = new ServicoModel
                        {
                            NomeDoServico = novoServico.NomeDoServico,
                            DescricaoDoServico = novoServico.DescricaoDoServico,
                            PrazoEntregaPadrao = novoServico.PrazoEntregaPadrao,
                            Fornecedor = fornecedorSelecionado
                        };

                        // Resto do código para salvar o serviço no banco de dados
                        // ...

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
    }
}
