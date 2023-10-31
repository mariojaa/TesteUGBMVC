//using Microsoft.AspNetCore.Mvc;
//using TesteUGB.Helper;
//using TesteUGB.Models;
//using TesteUGB.Repositorio;
//using TesteUGBMVC.Models;

//namespace TesteUGBMVC.Controllers
//{
//    public class AlterarSenhaController : Controller
//    {
//        private readonly string API_ENDPOINT = "http://localhost:9038/api/alterarsenha";
//        private readonly HttpClient httpClient;
//        private readonly UsuarioRepository _usuarioRepository;
//        private readonly ISessao _sessao;

//        public AlterarSenhaController(UsuarioRepository usuarioRepository, ISessao sessao)
//        {
//            httpClient = new HttpClient
//            {
//                BaseAddress = new Uri(API_ENDPOINT)
//            };
//            _usuarioRepository = usuarioRepository;
//            _sessao = sessao;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Alterar(AlterarSenhaModel alterarSenhaModel)
//        {
//            HttpResponseMessage response = await httpClient.GetAsync(API_ENDPOINT);
//            try
//            {
//                UsuarioModel usuarioLogado = _sessao.BuscarSessaoUsuario();
//                alterarSenhaModel.Id = usuarioLogado.Id;
//                if (ModelState.IsValid)
//                {
//                    _usuarioRepository.AlterarSenha(alterarSenhaModel);
//                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
//                    return RedirectToAction("Index");
//                }
//                return View(alterarSenhaModel);
//            }
//            catch (System.Exception erro)
//            {
//                TempData["MensagemErro"] = $"Ops, {erro.Message}";
//                return View(alterarSenhaModel);
//            }
//        }
//    }
//}
