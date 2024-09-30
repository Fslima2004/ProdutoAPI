using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutoAPI.Data;
using ProdutoAPI.Models;

namespace ProdutoAPI.Controllers
{
    // Definindo a rota do controlador como "api/produtos" e marcando como ApiController para habilitar comportamentos específicos de API
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // Dependência injetada do contexto do banco de dados ProdutoContext
        private readonly ProdutoContext _context;

        // Construtor que injeta a instância do contexto de banco de dados
        public ProdutosController(ProdutoContext context)
        {
            _context = context;
        }

        // Ação para retornar a lista de todos os produtos (GET: api/produtos)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            // Busca todos os produtos de forma assíncrona e retorna como uma lista
            return await _context.Produtos.ToListAsync();
        }

        // Ação para retornar um produto específico com base no ID (GET: api/produtos/{id})
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            // Busca o produto com o ID fornecido
            var produto = await _context.Produtos.FindAsync(id);

            // Retorna 404 (NotFound) se o produto não for encontrado
            if (produto == null)
            {
                return NotFound();
            }

            // Retorna o produto encontrado
            return produto;
        }

        // Ação para criar um novo produto (POST: api/produtos)
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            // Verifica se o estado do modelo é válido (validações aplicadas ao modelo)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Adiciona o produto ao contexto e salva as mudanças de forma assíncrona
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            // Retorna 201 (Created) e fornece a URL do novo recurso criado
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // Ação para atualizar um produto existente com base no ID (PUT: api/produtos/{id})
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            // Verifica se o ID na URL é o mesmo do objeto recebido
            if (id != produto.Id)
            {
                return BadRequest();
            }

            // Busca o produto existente no banco de dados
            var produtoExistente = await _context.Produtos.FindAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            // Atualiza as propriedades do produto existente com os novos valores
            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Preco = produto.Preco;
            produtoExistente.QuantidadeEmEstoque = produto.QuantidadeEmEstoque;

            // Salva as mudanças de forma assíncrona
            await _context.SaveChangesAsync();

            // Retorna 204 (NoContent) indicando que a operação foi bem-sucedida
            return NoContent();
        }

        // Ação para deletar um produto com base no ID (DELETE: api/produtos/{id})
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            // Busca o produto com o ID fornecido
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            // Remove o produto do contexto e salva as mudanças de forma assíncrona
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            // Retorna 204 (NoContent) indicando que a operação foi bem-sucedida
            return NoContent();
        }

        // Método privado auxiliar para verificar se um produto existe no banco de dados com base no ID
        private bool ProdutoExists(int id)
        {
            // Retorna true se o produto existir, false caso contrário
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
