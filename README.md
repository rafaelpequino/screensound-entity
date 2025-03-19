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

Este método recupera todos os artistas do banco de dados e os adiciona a uma lista, retornando-os como uma coleção de objetos `Artista`.

