using ProjetoCrud.Models;

namespace ProjetoCrud.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Usuario? Validar(string email, string senha);

        void CriarConta(LoginViewModel usuario);
    }
}
