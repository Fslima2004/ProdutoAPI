using ProdutoAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Obt�m a string de conex�o configurada no arquivo de configura��o (appsettings.json ou outros)
var connectionString = builder.Configuration.GetConnectionString("ProdutoContext");
Console.WriteLine($"Connection String: {connectionString}");

// Configura��o do contexto do banco de dados, utilizando o SQL Server com a string de conex�o fornecida
builder.Services.AddDbContext<ProdutoContext>(options =>
    options.UseSqlServer(connectionString));

// Configura��o de outros servi�os e middlewares
builder.Services.AddControllers(); // Adiciona os servi�os necess�rios para suportar controladores da API
builder.Services.AddEndpointsApiExplorer(); // Suporte para gera��o de endpoints da API
builder.Services.AddSwaggerGen(); // Configura o servi�o do Swagger para a documenta��o da API

var app = builder.Build(); // Constr�i o aplicativo com as configura��es e depend�ncias definidas

// Configura��o do pipeline de requisi��o HTTP
if (app.Environment.IsDevelopment())
{
    // Se estiver no ambiente de desenvolvimento, habilita o Swagger e a interface de usu�rio do Swagger para facilitar testes e documenta��o
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // For�a o redirecionamento de requisi��es HTTP para HTTPS por quest�es de seguran�a

app.UseAuthorization(); // Habilita a autoriza��o na aplica��o, para garantir que endpoints protegidos requerem autoriza��o

app.MapControllers(); // Mapeia as rotas dos controladores da API

app.Run(); // Executa a aplica��o
