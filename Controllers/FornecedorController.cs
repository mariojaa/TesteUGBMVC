using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TesteUGB.Models;
using TesteUGBMVC.Models;

namespace TesteUGBMVC.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/fornecedor";
        private readonly HttpClient httpClient;

        public FornecedorController()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
        }

        public async Task<IActionResult> ListaFornecedores()
        {
            List<FornecedorViewModel> fornecedores = null;
            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                fornecedores = JsonConvert.DeserializeObject<List<FornecedorViewModel>>(content);
                return View(fornecedores);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao processar a solicitação.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult CriarFornecedor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarFornecedor(FornecedorViewModel novoFornecedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fornecedorModel = new FornecedorModel
                    {
                        NomeEmpresaFornecedora = novoFornecedor.NomeEmpresaFornecedora,
                        EnderecoFornecedor = novoFornecedor.EnderecoFornecedor, // Corrigido aqui
                        NumeroEnderecoFornecedor = novoFornecedor.NumeroEnderecoFornecedor,
                        BairroEnderecoFornecedor = novoFornecedor.BairroEnderecoFornecedor,
                        CidadeEnderecoFornecedor = novoFornecedor.CidadeEnderecoFornecedor,
                        EstadoEnderecoFornecedor = novoFornecedor.EstadoEnderecoFornecedor,
                        PaisEnderecoFornecedor = novoFornecedor.PaisEnderecoFornecedor,
                        EmailFornecedor = novoFornecedor.EmailFornecedor,
                        CNPJFornecedor = novoFornecedor.CNPJFornecedor,
                        InscricaoEstadualEMunicipalFornecedor = novoFornecedor.InscricaoEstadualEMunicipalFornecedor
                    };

                    var novoFornecedorJson = JsonConvert.SerializeObject(fornecedorModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(novoFornecedorJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Fornecedor criado com sucesso!";
                        return RedirectToAction("ListaFornecedores");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao criar o fornecedor na API.");
                    }
                }
                catch (HttpRequestException ex) // Corrigido aqui
                {
                    ModelState.AddModelError("", "Erro ao criar o fornecedor: " + ex.Message);
                }
            }

            return View(novoFornecedor);
        }

        [HttpGet]
        public async Task<IActionResult> DeletarFornecedor(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Fornecedor excluído com sucesso!";
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao excluir o fornecedor na API.");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Erro ao excluir o fornecedor: " + ex.Message);
            }

            return RedirectToAction("ListaFornecedores");
        }

        [HttpGet]
        public async Task<IActionResult> EditarFornecedor(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var fornecedorModel = JsonConvert.DeserializeObject<FornecedorViewModel>(content);
                    return View(fornecedorModel);
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao obter o fornecedor da API.");
                    return View();
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Erro ao obter o fornecedor: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarFornecedor(FornecedorViewModel fornecedorEditado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fornecedorModel = new FornecedorModel
                    {
                        Id = fornecedorEditado.Id,
                        NomeEmpresaFornecedora = fornecedorEditado.NomeEmpresaFornecedora,
                        EnderecoFornecedor = fornecedorEditado.EnderecoFornecedor,
                        NumeroEnderecoFornecedor = fornecedorEditado.NumeroEnderecoFornecedor,
                        BairroEnderecoFornecedor = fornecedorEditado.BairroEnderecoFornecedor,
                        CidadeEnderecoFornecedor = fornecedorEditado.CidadeEnderecoFornecedor,
                        EstadoEnderecoFornecedor = fornecedorEditado.EstadoEnderecoFornecedor,
                        PaisEnderecoFornecedor = fornecedorEditado.PaisEnderecoFornecedor,
                        EmailFornecedor = fornecedorEditado.EmailFornecedor,
                        CNPJFornecedor = fornecedorEditado.CNPJFornecedor,
                        InscricaoEstadualEMunicipalFornecedor = fornecedorEditado.InscricaoEstadualEMunicipalFornecedor
                    };

                    var fornecedorJson = JsonConvert.SerializeObject(fornecedorModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(fornecedorJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{fornecedorModel.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Fornecedor editado com sucesso!";
                        return RedirectToAction("ListaFornecedores");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao editar o fornecedor na API.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError("", "Erro ao editar o fornecedor: " + ex.Message);
                }
            }
            return View(fornecedorEditado);
        }
    }
}
