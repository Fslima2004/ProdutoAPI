using ProdutoAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Obtém a string de conexão configurada no arquivo de configuração (appsettings.json ou outros)
var connectionString = builder.Configuration.GetConnectionString("ProdutoContext");
Console.WriteLine($"Connection String: {connectionString}");

// Configuração do contexto do banco de dados, utilizando o SQL Server com a string de conexão fornecida
builder.Services.AddDbContext<ProdutoContext>(options =>
    options.UseSqlServer(connectionString));

// Configuração de outros serviços e middlewares
builder.Services.AddControllers(); // Adiciona os serviços necessários para suportar controladores da API
builder.Services.AddEndpointsApiExplorer(); // Suporte para geração de endpoints da API
builder.Services.AddSwaggerGen(); // Configura o serviço do Swagger para a documentação da API

var app = builder.Build(); // Constrói o aplicativo com as configurações e dependências definidas

// Configuração do pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    // Se estiver no ambiente de desenvolvimento, habilita o Swagger e a interface de usuário do Swagger para facilitar testes e documentação
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Força o redirecionamento de requisições HTTP para HTTPS por questões de segurança

app.UseAuthorization(); // Habilita a autorização na aplicação, para garantir que endpoints protegidos requerem autorização

app.MapControllers(); // Mapeia as rotas dos controladores da API

app.Run(); // Executa a aplicação
