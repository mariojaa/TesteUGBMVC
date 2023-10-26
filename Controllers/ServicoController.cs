using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteUGB.Data;
using TesteUGB.Models;
using TesteUGBMVC.Models;


namespace TesteUGBMVC.Controllers
{
    public class ServicoController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/servico";
        private readonly HttpClient httpClient;

        public ServicoController()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
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
        public IActionResult CriarServico()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarServico(ServicoViewModel novoServico)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var servicoModel = new ServicoModel
                    {
                        NomeDoServico = novoServico.NomeDoServico,
                        DescricaoDoServico = novoServico.DescricaoDoServico,
                        PrazoEntregaPadrao = novoServico.PrazoEntregaPadrao
                    };

                    var novoServicoJson = JsonConvert.SerializeObject(servicoModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(novoServicoJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content); //Solicitaçõa Post

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Serviço criado com sucesso!";
                        return RedirectToAction("ListaServicos");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao criar o serviço na API.");
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
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}"); // solicitação para delete por ID

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
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}"); // solicitação para obter um usuário por ID

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var sericoModel = JsonConvert.DeserializeObject<ServicoViewModel>(content);
                    return View(sericoModel);
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

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{servicoModel.Id}", content); // Solicitação para editar o usuário por ID

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
