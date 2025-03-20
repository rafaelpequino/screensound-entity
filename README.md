# SCREENSOUND

## Conexão com Banco de Dados com Entity Framework
O **Entity Framework** é uma ORM (Object-Relational Mapping) que permite mapear um banco de dados relacional para uma aplicação orientada a objetos.

### 1. Instalar o Pacote Necessário  
Para conectar ao banco de dados utilizando Entity Framework, primeiro instale o pacote **Microsoft.EntityFrameworkCore.SqlServer**:

```sh
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 2. Encontrar a Connection String
Para encontrar a **Connection String** do banco de dados:

1. No **SQL Server Management Studio (SSMS)** ou outra ferramenta de gerenciamento do SQL Server, clique com o botão direito no banco de dados.
2. Selecione **Propriedades**.
3. Localize e copie a cadeia de conexão.

#### Exemplo de Connection String:
```plaintext
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=ScreenSound;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
Trust Server Certificate=False;
Application Intent=ReadWrite;
Multi Subnet Failover=False;
```

### 3. Criar o Contexto  
No **Entity Framework**, a conexão com o banco de dados é gerenciada através de um **Contexto**.

#### Passos:
1. Crie uma classe chamada **ScreenSoundContext.cs**
2. Herde a classe do **DbContext**
3. Defina a Connection String dentro do método `OnConfiguring`

#### Exemplo de Código:
```csharp
using Microsoft.EntityFrameworkCore;

namespace ScreenSound.Banco
{
    internal class ScreenSoundContext : DbContext
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScreenSound;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
```

### 4. Criar a Classe de Acesso a Dados (DAL)
Agora, criaremos a **Camada de Acesso a Dados (DAL)**, que será responsável pelas operações no banco de dados.

#### Exemplo de Código:
```csharp
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    internal class ArtistaDAL
    {
        private readonly ScreenSoundContext context;

        public ArtistaDAL(ScreenSoundContext context)
        {
            this.context = context;
        }

        public List<Artista> Listar()
        {
            return context.Artistas.ToList();
        }

        public void Adicionar(Artista artista)
        {
            context.Artistas.Add(artista);
            context.SaveChanges();
        }

        public void Atualizar(Artista artista)
        {
            context.Artistas.Update(artista);
            context.SaveChanges();
        }

        public void Deletar(Artista artista)
        {
            context.Artistas.Remove(artista);
            context.SaveChanges();
        }
    }
}
```

### 5. Criar o Modelo de Dados
Crie um modelo para representar os **Artistas** no banco de dados.

#### Exemplo de Código:
```csharp
namespace ScreenSound.Modelos
{
    public class Artista
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string FotoPerfil { get; set; }
        public string Bio { get; set; }
    }
}
```

### 6. Criar o Banco de Dados e as Tabelas
Agora, use **Migrations** para criar as tabelas no banco de dados automaticamente.

#### Passo 1: Criar a Primeira Migration
Execute o seguinte comando no terminal para criar uma **Migration**:
```sh
 dotnet ef migrations add CriarBancoDeDados
```

#### Passo 2: Atualizar o Banco de Dados
Após criar a migration, aplique-a no banco de dados:
```sh
 dotnet ef database update
```

Esse comando criará a estrutura do banco de dados conforme definido no **ScreenSoundContext** e nos modelos.

### 7. Executar Operações no Banco de Dados
Agora podemos executar operações básicas no banco de dados.

#### Adicionar um Novo Artista:
```csharp
using ScreenSound.Banco;
using ScreenSound.Modelos;

ScreenSoundContext context = new ScreenSoundContext();
ArtistaDAL artistaDAL = new ArtistaDAL(context);

var novoArtista = new Artista { Nome = "Artista Exemplo", FotoPerfil = "foto.jpg", Bio = "Biografia do artista" };
artistaDAL.Adicionar(novoArtista);
Console.WriteLine("Artista inserido com sucesso!");
```

#### Listar Todos os Artistas:
```csharp
List<Artista> artistas = artistaDAL.Listar();
foreach (var artista in artistas)
{
    Console.WriteLine($"ID: {artista.Id}, Nome: {artista.Nome}, Bio: {artista.Bio}");
}
```

#### Atualizar um Artista:
```csharp
var artistaParaAtualizar = artistas.FirstOrDefault(a => a.Nome == "Artista Exemplo");
if (artistaParaAtualizar != null)
{
    artistaParaAtualizar.Bio = "Nova biografia atualizada";
    artistaDAL.Atualizar(artistaParaAtualizar);
    Console.WriteLine("Artista atualizado com sucesso!");
}
```

#### Deletar um Artista:
```csharp
var artistaParaDeletar = artistas.FirstOrDefault(a => a.Nome == "Artista Exemplo");
if (artistaParaDeletar != null)
{
    artistaDAL.Deletar(artistaParaDeletar);
    Console.WriteLine("Artista removido com sucesso!");
}
```

### 8. Chamando os métodos dentro do projeto
Agora podemos chamar os métodos criados.

#### Exemplo de Uso:
```csharp
try
{
    var context = new ScreenSoundContext();
    var artistaDAL = new ArtistaDAL(context);

    var novoArtista = new Artista(Nome = "Gilberto Gil", FotoPerfil = "foto.jpg", Bio = "Biografia do grande Gilberto Gil atualizado");
    artistaDAL.Adicionar(novoArtista);

    var editarArtista = new Artista("Gilberto Gil", "Biografia do grande Gilberto Gil atualizado") { Id = 3002 };
    artistaDAL.Atualizar(novoArtista);
    artistaDAL.Deletar(novoArtista);

    var listaArtistas = artistaDAL.Listar();

    foreach (var artista in listaArtistas)
    {
        Console.WriteLine($"ID: {artista.Id}, Nome: {artista.Nome}, Bio: {artista.Bio}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```

---

## Conclusão
O **Entity Framework** simplifica a interação com o banco de dados, eliminando a necessidade de escrever SQL manualmente para operações CRUD. Com o **DbContext** e **Migrations**, é possível estruturar o banco de forma dinâmica e eficiente.
