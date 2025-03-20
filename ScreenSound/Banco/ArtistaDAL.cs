using Microsoft.Data.SqlClient;
using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    internal class ArtistaDAL
    {
        public List<Artista> Listar()
        {
            using var context = new ScreenSoundContext();
            return context.Artistas.ToList();
        }

        /*
        public void Adicionar(Artista artista)
        {
            using var connection = new ScreenSoundContext().ObterConexao();
            connection.Open();

            string sql = "INSERT INTO Artistas (Nome, FotoPerfil, Bio) VALUES (@nome, @perfilPadrao, @bio)";
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@nome", artista.Nome);
            command.Parameters.AddWithValue("@perfilPadrao", artista.FotoPerfil);
            command.Parameters.AddWithValue("@bio", artista.Bio);

            int retorno = command.ExecuteNonQuery();
            if (retorno > 0)
            {
                Console.WriteLine("Artista inserido com sucesso");
            }
        }*/
    }
}
