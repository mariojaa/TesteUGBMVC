//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.Net.Http.Headers;
//using System.Text;
//using TesteUGB.Helper;
//using TesteUGB.Models;
//using TesteUGB.Repositorio;
//using TesteUGB.Repository.Interface;
//using TesteUGBMVC.Models;

//namespace TesteUGBMVC.Controllers
//{
//    public class LoginController : Controller
//    {
//        private readonly string API_ENDPOINT = "http://localhost:9038/api/login";
//        private readonly HttpClient httpClient;
//        private readonly IUsuarioRepository _usuarioRepository;
//        private readonly ISessao _sessao;
//        private readonly IEmail _email;

//        public LoginController(IUsuarioRepository usuarioRepository, ISessao sessao, IEmail email)
//        {
//            httpClient = new HttpClient
//            {
//                BaseAddress = new Uri(API_ENDPOINT)
//            };
//            _usuarioRepository = usuarioRepository;
//            _sessao = sessao;
//            _email = email;
//        }

//        public IActionResult Index()
//        {
//            if (_sessao.BuscarSessaoUsuario() != null)
//            {
//                return RedirectToAction("Index", "Home");
//            }

//            return View();
//        }

//        public IActionResult Sair()
//        {
//            _sessao.RemoverSessaoUsuario();
//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Entrar(LoginViewModel novoLogin)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var loginModel = new UsuarioModel
//                    {
//                        UsuarioLogin = novoLogin.Login,
//                        Senha = novoLogin.Senha,
//                    };

//                    var novoUsuarioJson = JsonConvert.SerializeObject(loginModel);

//                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//                    var content = new StringContent(novoUsuarioJson, Encoding.UTF8, "application/json");

//                    HttpResponseMessage response = await httpClient.PostAsync(API_ENDPOINT, content); //Solicitaçõa Post

//                    if (response.IsSuccessStatusCode)
//                    {
//                        TempData["MensagemSucesso"] = "Login feito com sucesso!";
//                        return RedirectToAction("ListaUsuarios");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "Erro ao criar o usuário na API.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Erro ao criar o usuário: " + ex.Message);
//                }
//            }

//            return View(novoLogin);
//        }
//    }

//}
