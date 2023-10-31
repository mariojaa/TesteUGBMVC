//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using TesteUGB.Models;

//namespace TesteUGBMVC.ViewComponents
//{
//    public class Menu : ViewComponent
//    {
//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");
//            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
//            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
//            return View(usuario);
//        }
//    }
//}