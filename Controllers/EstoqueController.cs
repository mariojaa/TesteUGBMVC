using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteUGB.Models;
using TesteUGB.Repositorio;
using TesteUGBMVC.ViewModels;

namespace TesteUGBMVC.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/estoque";
        private readonly HttpClient httpClient;
        private readonly IEstoqueRepository _estoqueRepository;

        public EstoqueController(IEstoqueRepository estoqueRepository)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
            _estoqueRepository = estoqueRepository;
        }

        public async Task<IActionResult> ListaProdutos()
        {
            List<EstoqueViewModel> produtos = null;
            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                produtos = JsonConvert.DeserializeObject<List<EstoqueViewModel>>(content);
                return View(produtos);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao processar a solicitação.");
                return View();
            }
        }

        public IActionResult CriarProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto(EstoqueViewModel novoProduto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var produtoModel = new EstoqueModel
                    {
                        NomeProduto = novoProduto.NomeProduto,
                        QuantidadeTotalEmEstoque = (int)novoProduto.QuantidadeTotalEmEstoque,
                        QuantidadeMinimaEmEstoque = (int)novoProduto.QuantidadeMinimaEmEstoque,
                        SetorDeDeposito = novoProduto.SetorDeDeposito,
                        DataCadastroProduto = novoProduto.DataCadastroProduto,
                        TipoDoProdutoUnitarioOuPacote = novoProduto.TipoDoProdutoUnitarioOuPacote,
                        NumeroNotaFiscalProduto = novoProduto.NumeroNotaFiscalProduto,
                        CodigoEAN = novoProduto.CodigoEAN
                    };

                    var novoProdutoJson = JsonConvert.SerializeObject(produtoModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(novoProdutoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content);

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

        public async Task<IActionResult> DeletarProduto(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}");

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
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var produtoModel = JsonConvert.DeserializeObject<EstoqueViewModel>(content);
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
        public async Task<IActionResult> EditarProduto(EstoqueViewModel produtoEditado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var produtoModel = new EstoqueModel
                    {
                        Id = produtoEditado.Id,
                        NomeProduto = produtoEditado.NomeProduto,
                        QuantidadeTotalEmEstoque = (int)produtoEditado.QuantidadeTotalEmEstoque,
                        QuantidadeMinimaEmEstoque = (int)produtoEditado.QuantidadeMinimaEmEstoque,
                        SetorDeDeposito = produtoEditado.SetorDeDeposito,
                        DataCadastroProduto = produtoEditado.DataCadastroProduto,
                        TipoDoProdutoUnitarioOuPacote = produtoEditado.TipoDoProdutoUnitarioOuPacote,
                        NumeroNotaFiscalProduto = produtoEditado.NumeroNotaFiscalProduto,
                        CodigoEAN = produtoEditado.CodigoEAN
                    };

                    var produtoJson = JsonConvert.SerializeObject(produtoModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(produtoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{produtoModel.Id}", content);

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

        public async Task<IActionResult> EntradaProduto(int id)
        {
            var produto = await _estoqueRepository.FindById(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> EntradaProduto(int id, int quantidade)
        {
            try
            {
                var produto = await _estoqueRepository.FindById(id);

                if (produto == null)
                {
                    return NotFound();
                }

                if (quantidade <= 0)
                {
                    ModelState.AddModelError("quantidade", "A quantidade de entrada deve ser maior que zero.");
                    return View("Error");
                }

                // Adicione a quantidade à propriedade QuantidadeTotalEmEstoque
                produto.QuantidadeTotalEmEstoque += quantidade;

                await _estoqueRepository.Update(produto);

                return RedirectToAction("ListaProdutos");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> SaidaProduto(int id)
        {
            var produto = await _estoqueRepository.FindById(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> SaidaProduto(int id, int quantidade)
        {
            try
            {
                var produto = await _estoqueRepository.FindById(id);

                if (produto == null)
                {
                    return NotFound();
                }

                if (quantidade <= 0 || quantidade > produto.QuantidadeTotalEmEstoque)
                {
                    ModelState.AddModelError("quantidade", "A quantidade de saída não é válida.");
                    return View(produto);
                }

                // Subtraia a quantidade da propriedade QuantidadeTotalEmEstoque
                produto.QuantidadeTotalEmEstoque -= quantidade;

                await _estoqueRepository.Update(produto);

                return RedirectToAction("ListaProdutos");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPatch("{id}/{acao}")]
        public async Task<ActionResult<EstoqueModel>> AtualizarEstoque(int id, string acao, [FromBody] EstoqueViewModel model)
        {
            var produto = await _estoqueRepository.FindById(id);

            if (produto == null)
            {
                return NotFound();
            }

            if (acao == "entrada")
            {
                if (model.Quantidade <= 0)
                {
                    return BadRequest("A quantidade de entrada deve ser maior que zero.");
                }

                produto.QuantidadeTotalEmEstoque += model.Quantidade;
            }
            else if (acao == "saida")
            {
                if (model.Quantidade <= 0 || model.Quantidade > produto.QuantidadeTotalEmEstoque)
                {
                    return BadRequest("A quantidade de saída não é válida.");
                }

                produto.QuantidadeTotalEmEstoque -= model.Quantidade;
            }
            else
            {
                return BadRequest("Ação inválida.");
            }

            try
            {
                await _estoqueRepository.Update(produto);
                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ops, sem conexão com o banco de dados! Aguarde alguns minutos e tente novamente.");
            }
        }
    }
}
