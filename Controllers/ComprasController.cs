using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TesteUGB.Models;
using TesteUGB.Repositories;
using TesteUGBMVC.Models;
using TesteUGBMVC.ViewModels;

namespace TesteUGBMVC.Controllers
{
    [Route("api/compras")]
    public class ComprasController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/compras";
        private readonly HttpClient httpClient;
        private readonly ComprasRepository _comprasRepository;

        public ComprasController(ComprasRepository comprasRepository)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
            _comprasRepository = comprasRepository;
        }

        [HttpGet("ListaCompras")]
        public async Task<IActionResult> ListaCompras()
        {
            List<ComprasViewModel> compras = null;
            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                compras = JsonConvert.DeserializeObject<List<ComprasViewModel>>(content);
                return View(compras);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao processar a solicitação.");
                return View();
            }
        }

        [HttpGet("CriarCompra")]
        public IActionResult CriarCompra()
        {
            return View();
        }

        [HttpPost("CriarCompra")]
        public async Task<IActionResult> CriarCompra(ComprasViewModel novaCompra)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var compraModel = new ComprasModel
                    {
                        NomeProduto = novaCompra.NomeProduto,
                        CodigoDaSolicitacao = novaCompra.CodigoDaSolicitacao,
                        Fabricante = novaCompra.Fabricante,
                        QuantidadeSolicitada = novaCompra.QuantidadeSolicitada,
                        DepartamentoSolicitante = novaCompra.DepartamentoSolicitante,
                        DataSolicitada = novaCompra.DataSolicitada,
                        DataPrevisaoEntregaProduto = novaCompra.DataPrevisaoEntregaProduto,
                        TipoDoProduto = novaCompra.TipoDoProduto,
                        ValorUnitarioDoProduto = novaCompra.ValorUnitarioDoProduto,
                        NumeroNotaFiscalProduto = novaCompra.NumeroNotaFiscalProduto,
                        CodigoEAN = novaCompra.CodigoEAN
                    };

                    var novaCompraJson = JsonConvert.SerializeObject(compraModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(novaCompraJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Compra cadastrada com sucesso!";
                        return RedirectToAction("ListaCompras");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao criar a compra na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao criar a compra: " + ex.Message);
                }
            }

            return View(novaCompra);
        }

        [HttpGet("DeletarCompra/{id}")]
        public async Task<IActionResult> DeletarCompra(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Compra excluída com sucesso!";
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao excluir a compra na API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir a compra: " + ex.Message);
            }

            return RedirectToAction("ListaCompras");
        }
        [HttpGet]
        public async Task<IActionResult> EditarCompra(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var compraModel = JsonConvert.DeserializeObject<ComprasViewModel>(content);
                    return View(compraModel);
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao obter o usuário da API.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao obter o usuário: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarCompra(ComprasViewModel compraEditada)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var compraModel = new ComprasModel
                    {
                        Id = compraEditada.Id,
                        NomeProduto = compraEditada.NomeProduto,
                        CodigoDaSolicitacao = compraEditada.CodigoDaSolicitacao,
                        Fabricante = compraEditada.Fabricante,
                        QuantidadeSolicitada = compraEditada.QuantidadeSolicitada,
                        DepartamentoSolicitante = compraEditada.DepartamentoSolicitante,
                        DataSolicitada = compraEditada.DataSolicitada,
                        DataPrevisaoEntregaProduto = compraEditada.DataPrevisaoEntregaProduto,
                        TipoDoProduto = compraEditada.TipoDoProduto,
                        ValorUnitarioDoProduto = compraEditada.ValorUnitarioDoProduto,
                        NumeroNotaFiscalProduto = compraEditada.NumeroNotaFiscalProduto,
                        CodigoEAN = compraEditada.CodigoEAN
                    };

                    var compraJson = JsonConvert.SerializeObject(compraModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(compraJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{compraModel.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Compra editada com sucesso!";
                        return RedirectToAction("ListaUsuarios");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao editar a compra na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao editar o usuário: " + ex.Message);
                }
            }

            return View(compraEditada);
        }
    }
}
