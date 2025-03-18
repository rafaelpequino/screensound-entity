# SCREENSOUND

## Conexão com Banco de Dados

### 1. Instalar o Pacote Necessário  
Baixe e instale o pacote `Microsoft.Data.SqlClient` para permitir a conexão com o banco de dados.

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
