using MySql.Data.MySqlClient;
using ProjetoCrud.Models;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;
using BCrypt.Net;

namespace ProjetoCrud.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration config) =>
            _connectionString = config.GetConnectionString("Conexao");

        public Usuario? Validar(string email, string senha)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Usuarios WHERE Email = @e AND Senha = @s";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@s", senha);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Nivel = reader["Nivel"].ToString()!
                };
            }
            return null;
        }

        public void CriarConta(LoginViewModel usuario)
        {
            using (var conn=new MySqlConnection(_connectionString))
            {
                conn.Open();
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

                var sql = "INSERT INTO Usuarios(Nome,Email,Senha,Nivel)VALUES(@n,@e,@s,@l)";
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@n", usuario.Nome);
                cmd.Parameters.AddWithValue("@e", usuario.Email);
                cmd.Parameters.AddWithValue("@s", senhaHash);
                cmd.Parameters.AddWithValue("@l", "Usuario");
                cmd.ExecuteNonQuery();
            }
        }
    }
}
