using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class ArtistasExtensions
    {
        public static void AddEndPointsArtistas(this WebApplication app)
        {

            app.MapGet("/Artistas", () =>
            {
                var dal = new DAL<Artista>(new ScreenSoundContext());
                return Results.Ok(dal.Listar());
            });

            app.MapGet("/Artistas/{nome}", (string nome) =>
            {
                var dal = new DAL<Artista>(new ScreenSoundContext());
                var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
                if (artista is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(artista);
            });

            app.MapPost("/Artistas", ([FromBody] Artista artista) =>
            {
                var dal = new DAL<Artista>(new ScreenSoundContext());
                dal.Adicionar(artista);
                return Results.Ok();
            });

            app.MapDelete("/Artistas/{id}", (int id) =>
            {
                var dal = new DAL<Artista>(new ScreenSoundContext());
                var artista = dal.RecuperarPor(a => a.Id == id);
                if (artista is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(artista);
                return Results.NoContent();
            });

            app.MapPut("/Artistas", ([FromBody] Artista artista) =>
            {
                var dal = new DAL<Artista>(new ScreenSoundContext());
                var artistaAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);
                if (artistaAtualizar is null)
                {
                    return Results.NotFound();
                }
                artistaAtualizar.Nome = artista.Nome;
                artistaAtualizar.Bio = artista.Bio;
                artistaAtualizar.FotoPerfil = artista.FotoPerfil;

                dal.Atualizar(artistaAtualizar);
                return Results.Ok();
            });

        }
    }
}
