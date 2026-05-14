using MySql.Data.MySqlClient;
using ProjetoCrud.Models;
using System.Data;

namespace ProjetoCrud.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString;

        public ProdutoRepositorio(IConfiguration config)
        {
            _connectionString=config.GetConnectionString("Conexao");
        }

        public IEnumerable<Produto> ListarTodos()
        {
            var lista = new List<Produto>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT*FROM Produtos", conn);
            using var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                lista.Add(new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"])
                });
            }
            return lista;
        }
        public Produto? ObterPorId( int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Proddutos WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"])
                };
            }
            return null;
        }
        public void Adicionar(Produto p)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO Produtos (Nome, Preco) VALUES (@n, @p)", conn);
            cmd.Parameters.AddWithValue("@n", p.Nome);
        }
    }
}
