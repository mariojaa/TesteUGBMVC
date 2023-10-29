using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteUGB.Data;
using TesteUGB.Models;
using TesteUGBMVC.Models;


namespace TesteUGBMVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string API_ENDPOINT = "http://localhost:9038/api/usuario";
        private readonly HttpClient httpClient;

        public UsuarioController(TesteUGBDbContext context)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_ENDPOINT)
            };
        }

        public async Task<IActionResult> ListaUsuarios()
        {
            List<UsuarioViewModel> usuarios = null;
            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                usuarios = JsonConvert.DeserializeObject<List<UsuarioViewModel>>(content);
                return View(usuarios);
            }
            else
            {
                ModelState.AddModelError("", "Erro ao processar a solicitação.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult CriarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioViewModel novoUsuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioModel = new UsuarioModel
                    {
                        MatriculaUsuario = novoUsuario.MatriculaUsuario,
                        NomeCompletoUsuario = novoUsuario.NomeCompletoUsuario,
                        EmailUsuario = novoUsuario.EmailUsuario,
                        UsuarioLogin = novoUsuario.UsuarioLogin,
                        DepartamentoFuncionario = novoUsuario.DepartamentoFuncionario
                    };

                    var novoUsuarioJson = JsonConvert.SerializeObject(usuarioModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(novoUsuarioJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content); //Solicitaçõa Post

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Usuário criado com sucesso!";
                        return RedirectToAction("ListaUsuarios");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao criar o usuário na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao criar o usuário: " + ex.Message);
                }
            }

            return View(novoUsuario);
        }

        [HttpGet]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{API_ENDPOINT}/{id}"); // solicitação para delete por ID

                if (response.IsSuccessStatusCode)
                {
                    TempData["MensagemSucesso"] = "Usuário excluído com sucesso!";
                }
                else
                {
                    ModelState.AddModelError("", "Erro ao excluir o usuário na API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir o usuário: " + ex.Message);
            }

            return RedirectToAction("ListaUsuarios");
        }
        [HttpGet]
        public async Task<IActionResult> EditarUsuario(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var usuarioModel = JsonConvert.DeserializeObject<UsuarioViewModel>(content);
                    return View(usuarioModel);
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
        public async Task<IActionResult> EditarUsuario(UsuarioViewModel usuarioEditado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioModel = new UsuarioModel
                    {
                        Id = usuarioEditado.Id,
                        MatriculaUsuario = usuarioEditado.MatriculaUsuario,
                        NomeCompletoUsuario = usuarioEditado.NomeCompletoUsuario,
                        EmailUsuario = usuarioEditado.EmailUsuario,
                        UsuarioLogin = usuarioEditado.UsuarioLogin,
                        DepartamentoFuncionario = usuarioEditado.DepartamentoFuncionario
                    };

                    var usuarioJson = JsonConvert.SerializeObject(usuarioModel);

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PutAsync($"{API_ENDPOINT}/{usuarioModel.Id}", content); // Solicitação para editar o usuário por ID

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["MensagemSucesso"] = "Usuário editado com sucesso!";
                        return RedirectToAction("ListaUsuarios");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao editar o usuário na API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao editar o usuário: " + ex.Message);
                }
            }

            return View(usuarioEditado);
        }
    }
}
