using Microsoft.EntityFrameworkCore;
using Xunit;
using ProdutoAPI.Controllers;
using ProdutoAPI.Data;
using ProdutoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ProdutoAPI.Tests
{
    public class ProdutosControllerTests
    {
        private readonly DbContextOptions<ProdutoContext> _options;

        public ProdutosControllerTests()
        {
            // Configurando um banco de dados em memória com um nome único para cada teste
            _options = new DbContextOptionsBuilder<ProdutoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task PostProduto_CreatesProduto()
        {
            // Arrange
            using (var context = new ProdutoContext(_options))
            {
                var controller = new ProdutosController(context);
                var novoProduto = new Produto { Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 10.0m, QuantidadeEmEstoque = 5 };

                // Act
                var result = await controller.PostProduto(novoProduto);

                // Assert
                var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var produtoRetornado = Assert.IsType<Produto>(createdResult.Value);
                Assert.Equal(novoProduto.Nome, produtoRetornado.Nome);
            }
        }

        [Fact]
        public async Task GetProdutos_ReturnsAllProdutos()
        {
            // Arrange
            using (var context = new ProdutoContext(_options))
            {
                context.Produtos.Add(new Produto { Nome = "Produto 1", Descricao = "Descrição 1", Preco = 10.0m, QuantidadeEmEstoque = 5 });
                context.Produtos.Add(new Produto { Nome = "Produto 2", Descricao = "Descrição 2", Preco = 20.0m, QuantidadeEmEstoque = 10 });
                context.SaveChanges(); // Persistir as mudanças no banco de dados em memória

                var controller = new ProdutosController(context);

                // Act
                var result = await controller.GetProdutos();

                // Assert
                var okResult = Assert.IsType<ActionResult<IEnumerable<Produto>>>(result);
                var produtos = Assert.IsAssignableFrom<IEnumerable<Produto>>(okResult.Value);
                Assert.Equal(2, produtos.Count()); // Verifica se retornou 2 produtos
            }
        }

        [Fact]
        public async Task GetProduto_ReturnsProduto()
        {
            // Arrange
            using (var context = new ProdutoContext(_options))
            {
                var produto = new Produto { Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 10.0m, QuantidadeEmEstoque = 5 };
                context.Produtos.Add(produto);
                context.SaveChanges();

                var controller = new ProdutosController(context);

                // Act
                var result = await controller.GetProduto(produto.Id);

                // Assert
                var okResult = Assert.IsType<ActionResult<Produto>>(result);
                var produtoRetornado = Assert.IsType<Produto>(okResult.Value);
                Assert.Equal(produto.Nome, produtoRetornado.Nome);
            }
        }

        [Fact]
        public async Task PutProduto_UpdatesProduto()
        {
            // Arrange
            using (var context = new ProdutoContext(_options))
            {
                var produto = new Produto { Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 10.0m, QuantidadeEmEstoque = 5 };
                context.Produtos.Add(produto);
                context.SaveChanges();

                var controller = new ProdutosController(context);
                var produtoAtualizado = new Produto { Id = produto.Id, Nome = "Produto Atualizado", Descricao = "Descrição Atualizada", Preco = 15.0m, QuantidadeEmEstoque = 10 };

                // Act
                var result = await controller.PutProduto(produto.Id, produtoAtualizado);

                // Assert
                Assert.IsType<NoContentResult>(result);
                var produtoRetornado = await context.Produtos.FindAsync(produto.Id);
                Assert.Equal(produtoAtualizado.Nome, produtoRetornado.Nome);
            }
        }

        [Fact]
        public async Task DeleteProduto_RemovesProduto()
        {
            // Arrange
            using (var context = new ProdutoContext(_options))
            {
                var produto = new Produto { Nome = "Produto Teste", Descricao = "Descrição Teste", Preco = 10.0m, QuantidadeEmEstoque = 5 };
                context.Produtos.Add(produto);
                context.SaveChanges();

                var controller = new ProdutosController(context);

                // Act
                var result = await controller.DeleteProduto(produto.Id);

                // Assert
                Assert.IsType<NoContentResult>(result);
                var produtoRetornado = await context.Produtos.FindAsync(produto.Id);
                Assert.Null(produtoRetornado); // Verifica se o produto foi removido
            }
        }
    }
}
