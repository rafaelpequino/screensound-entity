# SCREENSOUND

## Utilização de Entity Framework

### Conexão com Banco de Dados com Entity Framework
O **Entity Framework** é uma ORM (Object-Relational Mapping) que permite mapear um banco de dados relacional para uma aplicação orientada a objetos.

#### 1. Instalar o Pacote Necessário  
Para conectar ao banco de dados utilizando Entity Framework, primeiro instale o pacote **Microsoft.EntityFrameworkCore.SqlServer**:

```sh
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

#### 2. Encontrar a Connection String
Para encontrar a **Connection String** do banco de dados:

1. No **SQL Server Management Studio (SSMS)** ou outra ferramenta de gerenciamento do SQL Server, clique com o botão direito no banco de dados.
2. Selecione **Propriedades**.
3. Localize e copie a cadeia de conexão.

###### Exemplo de Connection String:
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

#### 3. Criar o Contexto  
No **Entity Framework**, a conexão com o banco de dados é gerenciada através de um **Contexto**.

###### Exemplo de Código:
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

#### 4. Criar a Classe de Acesso a Dados (DAL)
Agora, criaremos a **Camada de Acesso a Dados (DAL)**, que será responsável pelas operações no banco de dados.

###### Exemplo de Código:
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

#### 5. Criar o Modelo de Dados
Crie um modelo para representar os **Artistas** no banco de dados.

###### Exemplo de Código:
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

#### 6. Criar o Banco de Dados e as Tabelas
Agora, use **Migrations** para criar as tabelas no banco de dados automaticamente.

```sh
dotnet ef migrations add CriarBancoDeDados
dotnet ef database update
```

#### 7. Executar Operações no Banco de Dados
###### Adicionar um Novo Artista:
```csharp
using ScreenSound.Banco;
using ScreenSound.Modelos;

ScreenSoundContext context = new ScreenSoundContext();
ArtistaDAL artistaDAL = new ArtistaDAL(context);

var novoArtista = new Artista { Nome = "Artista Exemplo", FotoPerfil = "foto.jpg", Bio = "Biografia do artista" };
artistaDAL.Adicionar(novoArtista);
Console.WriteLine("Artista inserido com sucesso!");
```

###### Listar Todos os Artistas:
```csharp
List<Artista> artistas = artistaDAL.Listar();
foreach (var artista in artistas)
{
    Console.WriteLine($"ID: {artista.Id}, Nome: {artista.Nome}, Bio: {artista.Bio}");
}
```

###### Atualizar um Artista:
```csharp
var artistaParaAtualizar = artistas.FirstOrDefault(a => a.Nome == "Artista Exemplo");
if (artistaParaAtualizar != null)
{
    artistaParaAtualizar.Bio = "Nova biografia atualizada";
    artistaDAL.Atualizar(artistaParaAtualizar);
    Console.WriteLine("Artista atualizado com sucesso!");
}
```

###### Deletar um Artista:
```csharp
var artistaParaDeletar = artistas.FirstOrDefault(a => a.Nome == "Artista Exemplo");
if (artistaParaDeletar != null)
{
    artistaDAL.Deletar(artistaParaDeletar);
    Console.WriteLine("Artista removido com sucesso!");
}
```

#### 8. Chamando os métodos dentro do projeto
```csharp
try
{
    var context = new ScreenSoundContext();
    var artistaDAL = new ArtistaDAL(context);

    var novoArtista = new Artista { Nome = "Gilberto Gil", FotoPerfil = "foto.jpg", Bio = "Biografia do grande Gilberto Gil" };
    artistaDAL.Adicionar(novoArtista);

    var editarArtista = new Artista { Id = 3002, Nome = "Gilberto Gil", Bio = "Biografia atualizada" };
    artistaDAL.Atualizar(editarArtista);
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

### Utilização de Generics na Camada DAL

Para reutilizar a lógica da DAL para diferentes entidades do banco de dados, podemos usar **Generics**. Assim, com uma única classe, é possível realizar operações CRUD em qualquer entidade.

###### Exemplo de DAL Genérica:
```csharp
using ScreenSound.Modelos;

namespace ScreenSound.Banco
{
    internal class DAL<T> where T : class
    {
        protected readonly ScreenSoundContext context;

        protected DAL(ScreenSoundContext context)
        {
            this.context = context;
        }

        public List<T> Listar()
        {
            return context.Set<T>().ToList();
        }

        public void Adicionar(T objeto)
        {
            context.Set<T>().Add(objeto);
            context.SaveChanges();
        }

        public void Atualizar(T objeto)
        {
            context.Set<T>().Update(objeto);
            context.SaveChanges();
        }

        public void Deletar(T objeto)
        {
            context.Set<T>().Remove(objeto);
            context.SaveChanges();
        }

        public T? RecuperarPor(Func<T, bool> condicao)
        {
            return context.Set<T>().FirstOrDefault(condicao);
        }
    }
}
```

Com essa abordagem, basta criar uma nova instância de `DAL<T>` com a entidade desejada (por exemplo: `DAL<Artista>`) e todas as operações estarão disponíveis.

---

### Conclusão
O **Entity Framework** simplifica a interação com o banco de dados, eliminando a necessidade de escrever SQL manualmente para operações CRUD. Com o **DbContext**, **Migrations** e o uso de **Generics**, é possível estruturar e acessar o banco de forma dinâmica, segura e reutilizável.
