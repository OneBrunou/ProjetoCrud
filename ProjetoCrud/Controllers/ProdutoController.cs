using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoCrud.Models;
using ProjetoCrud.Repositorio;

namespace ProjetoCrud.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            var produtos = _produtoRepositorio.ListarTodos();
            return View(produtos);
        }
        [HttpGet]
        public IActionResult Criar() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Produto pd)
        {
            if (!ModelState.IsValid) return View(pd);
            var produto = new Produto
            {
                Nome = pd.Nome,
                Preco=pd.Preco
            };
            _produtoRepositorio.Adicionar(produto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var produto = _produtoRepositorio.ObterPorId(id);
            if (produto == null) return NotFound();
            var viewModel = new Produto
            {
                Id = produto.Id,
                Nome=produto.Nome,
                Preco=produto.Preco
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Produto model)
        {
            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var produto = new Produto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Preco = model.Preco
                };
                _produtoRepositorio.Atualizar(produto);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            _produtoRepositorio.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
