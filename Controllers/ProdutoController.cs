using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteUGB.Models;
using TesteUGBMVC.Models;


namespace TesteUGBMVC.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/produto";
        private readonly HttpClient httpClient;

        public ProdutoController()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
        }

        public async Task<IActionResult> ListaProdutos()
        {
            List<ProdutoViewModel> produtos = null;
            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                produtos = JsonConvert.DeserializeObject<List<ProdutoViewModel>>(content);
                return View(produtos);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao processar a solicitação.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult CrairProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto(ProdutoViewModel novoProduto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var produtoModel = new ProdutoModel
                    {
                        NomeProduto = novoProduto.NomeProduto,
                        NumeroPedidoProduto = novoProduto.NumeroPedidoProduto,
                        FornecedorProduto = novoProduto.FornecedorProduto,
                        QuantidadeEmEstoque = novoProduto.QuantidadeEmEstoque,
                        QuantidadeMinimaEmEstoque = novoProduto.QuantidadeMinimaEmEstoque,
                        SetorDeDeposito = novoProduto.SetorDeDeposito,
                        DataCadastroProduto = novoProduto.DataCadastroProduto,
                        DataPrevisaoEntregaProduto = novoProduto.DataPrevisaoEntregaProduto,
                        TipoDoProdutoUnitarioOuPacote = novoProduto.TipoDoProdutoUnitarioOuPacote,
                        ValorUnitarioDoProduto = novoProduto.ValorUnitarioDoProduto,
                        NumeroNotaFiscalProduto = novoProduto.NumeroNotaFiscalProduto,
                        CodigoEAN = novoProduto.CodigoEAN
                    };

                    var novoProdutoJson = JsonConvert.SerializeObject(produtoModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(novoProdutoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content); //Solicitaçõa Post

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!";
                        return RedirectToAction("ListaProdutos");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao criar o produto na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao criar o produto: " + ex.Message);
                }
            }

            return View(novoProduto);
        }

        [HttpGet]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}"); // solicitação para delete por ID

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Produto excluído com sucesso!";
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao excluir o produto na API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir o produto: " + ex.Message);
            }

            return RedirectToAction("ListaProdutos");
        }
        [HttpGet]
        public async Task<IActionResult> EditarProduto(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}"); // solicitação para obter um produto por ID

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var produtoModel = JsonConvert.DeserializeObject<ProdutoViewModel>(content);
                    return View(produtoModel);
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao obter o produto da API.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao obter o produto: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarProduto(ProdutoViewModel produtoEditado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var produtoModel = new ProdutoModel
                    {
                        Id = produtoEditado.Id,
                        NomeProduto = produtoEditado.NomeProduto,
                        NumeroPedidoProduto = produtoEditado.NumeroPedidoProduto,
                        FornecedorProduto = produtoEditado.FornecedorProduto,
                        QuantidadeEmEstoque = produtoEditado.QuantidadeEmEstoque,
                        QuantidadeMinimaEmEstoque = produtoEditado.QuantidadeMinimaEmEstoque,
                        SetorDeDeposito = produtoEditado.SetorDeDeposito,
                        DataCadastroProduto = produtoEditado.DataCadastroProduto,
                        DataPrevisaoEntregaProduto = produtoEditado.DataPrevisaoEntregaProduto,
                        TipoDoProdutoUnitarioOuPacote = produtoEditado.TipoDoProdutoUnitarioOuPacote,
                        ValorUnitarioDoProduto = produtoEditado.ValorUnitarioDoProduto,
                        NumeroNotaFiscalProduto = produtoEditado.NumeroNotaFiscalProduto,
                        CodigoEAN = produtoEditado.CodigoEAN
                    };

                    var produtoJson = JsonConvert.SerializeObject(produtoModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(produtoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{produtoModel.Id}", content); // Solicitação para editar o usuário por ID

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Produto editado com sucesso!";
                        return RedirectToAction("ListaProdutos");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao editar o produto na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao editar o produto: " + ex.Message);
                }
            }
            return View(produtoEditado);
        }
    }
}
