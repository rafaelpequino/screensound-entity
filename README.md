# SCREENSOUND

## Conexão com Banco de Dados ADO.NET

### 1. Instalar o Pacote Necessário  
Para conectar ao banco de dados, primeiro instale o pacote **Microsoft.Data.SqlClient**:

```sh
 dotnet add package Microsoft.Data.SqlClient
```

### 2. Obter a Cadeia de Conexão  
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

### 3. Criar a Conexão  
Para estabelecer a conexão com o banco de dados em C#, siga estes passos:

1. Defina uma string privada para armazenar a **Connection String**.
2. Crie um método público que retorne um `SqlConnection` a partir dessa string.

#### Exemplo de Código:
```csharp
using System;
using System.Data.SqlClient;

public class DatabaseConnection
{
    private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScreenSound;Integrated Security=True;";

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}
```

### 4. Abrir a Conexão  
Para acessar o banco de dados, utilize um `try-catch` para garantir que a conexão seja aberta corretamente:

#### Exemplo de Código:
```csharp
public void OpenConnection()
{
    try
    {
        using (SqlConnection connection = GetConnection())
        {
            connection.Open();
            Console.WriteLine("Conexão aberta com sucesso!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao conectar: " + ex.Message);
    }
}
```

### 5. Ler Dados do Banco de Dados  
Agora, vamos criar um método para listar todos os artistas armazenados no banco de dados.

#### Exemplo de Código:
```csharp
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class DatabaseConnection
{
    private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScreenSound;Integrated Security=True;";

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }

    public List<Artista> Listar()
    {
        var lista = new List<Artista>();
        using var connection = GetConnection();
        connection.Open();

        string sql = "SELECT * FROM Artistas";
        SqlCommand command = new SqlCommand(sql, connection);
        using SqlDataReader dataReader = command.ExecuteReader();

        while (dataReader.Read())
        {
            string nomeArtista = Convert.ToString(dataReader["Nome"]);
            string bioArtista = Convert.ToString(dataReader["Bio"]);
            int idArtista = Convert.ToInt32(dataReader["Id"]);
            Artista artista = new(nomeArtista, bioArtista) { Id = idArtista };

            lista.Add(artista);    
        }

        return lista;
    }
}
```

### 6. Inserir Dados no Banco de Dados
Agora, vamos adicionar um método para inserir novos artistas no banco de dados.

#### Exemplo de Código:
```csharp
public void Adicionar(Artista artista)
{
    using var connection = new Connection().ObterConexao();
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
}
```

### ADO.NET e a Camada de Acesso a Dados (DAL)
O **ADO.NET** é um conjunto de classes que permitem acesso a dados em aplicações .NET. Ele possibilita manipular dados de maneira consistente, separando o acesso aos dados da lógica do sistema.

Os principais objetos utilizados no ADO.NET são:
- **SqlConnection**: Representa a conexão com o banco de dados.
- **SqlCommand**: Representa a instrução SQL que será executada no banco de dados.
- **SqlDataReader**: Fornece um modo de ler as linhas do banco de dados de forma eficiente.

### DAO vs DAL
Se você já ouviu falar em **DAO** (Data Access Object) e **DAL** (Data Access Layer), pode se perguntar qual a diferença entre os dois.

- **DAO** é um objeto do banco de dados que representa um banco aberto.
- **DAL** é a camada de acesso a dados, promovendo a abstração dos dados e separando a lógica do banco de dados da lógica de negócios. O DAL é independente da fonte de dados, permitindo maior flexibilidade e manutenibilidade no código.