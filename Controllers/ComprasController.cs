using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteUGB.Models;
using TesteUGB.Repositories;
using TesteUGB.Repositorio;
using TesteUGB.Repository.Interface;
using TesteUGBMVC.Models;
using System.Globalization; // Adicione esta importação para trabalhar com valores monetários
using TesteUGBMVC.ViewModels;

namespace TesteUGBMVC.Controllers
{
    [Route("api/compras")]
    public class ComprasController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/compras";
        private readonly HttpClient httpClient;
        private readonly ComprasRepository _comprasRepository;
        private readonly IEmail _email;
        private readonly IUsuarioRepository _usuarioRepository;

        public ComprasController(ComprasRepository comprasRepository, IEmail email, IUsuarioRepository usuarioRepository)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
            _comprasRepository = comprasRepository;
            _email = email;
            _usuarioRepository = usuarioRepository;
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
        public async Task<IActionResult> CriarCompra(ComprasViewModel novaCompra, UsuarioModel usuarioViewModel)
        {
            if (!ModelState.IsValid)
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

                   /* UsuarioModel usuario = await _usuarioRepository.BuscarPorEmailELogin(usuarioViewModel.EmailUsuario, usuarioViewModel.UsuarioLogin)*/;

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content);
                    //string mensagemEmail = "Você tem uma nova solicitação de compra!";
                    //bool emailEnviado = _email.EnviarEmail(usuario.EmailUsuario, "Novo Pedido de Compra", mensagemEmail);

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

        [HttpGet("EditarCompra/{id}")]
        public async Task<IActionResult> EditarCompra(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var produtoModel = JsonConvert.DeserializeObject<ComprasViewModel>(content);
                    return View(produtoModel);
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao obter a compra da API.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao obter a compra: " + ex.Message);
                return View();
            }
        }

        [HttpPost("EditarCompra/{id}")]
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
                        ValorUnitarioDoProduto = compraEditada.ValorUnitarioDoProduto, //cultureinfo
                        ValorTotal = compraEditada.ValorTotal, //cultoreinfo
                        NumeroNotaFiscalProduto = compraEditada.NumeroNotaFiscalProduto,
                        CodigoEAN = compraEditada.CodigoEAN
                        
                    };

                    var produtoJson = JsonConvert.SerializeObject(compraModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(produtoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{compraModel.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Compra editada com sucesso!";
                        return RedirectToAction("ListaCompras");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao editar a compra na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao editar a compra: " + ex.Message);
                }
            }
            return View(compraEditada);
        }
    }
}
