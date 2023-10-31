using TesteUGB.Models.Enum;

namespace TesteUGBMVC.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public int MatriculaUsuario { get; set; }
        public string NomeCompletoUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string UsuarioLogin { get; set; }
        public string DepartamentoFuncionario { get; set; }
        //public string Senha { get; set; }
        //public PerfilEnum Perfil { get; set; }
    }
}
