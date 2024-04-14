using Importa_planilha_excel.Data;
using Importa_planilha_excel.Models;
using Importa_planilha_excel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;



namespace Importa_planilha_excel.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExcelInterface _excelInterface;
        public HomeController(AppDbContext appContext, IExcelInterface excelInterface)
        {
            _context = appContext;
            _excelInterface = excelInterface;
        }
        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return View(produtos);
        }
        [HttpPost]
        public IActionResult ImportaExcel(IFormFile form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var streamFile = _excelInterface.LerStream(form);
                    var produtos = _excelInterface.LerXls(streamFile);
                    _excelInterface.SalvarDados(produtos);

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return RedirectToAction("Index");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirTodos()
        {
            try
            {
                var produtos = await _context.Produtos.ToListAsync();
                if (produtos == null || produtos.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                foreach (var produto in produtos)
                {
                   _context.Produtos.Remove(produto);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult ExportarArquivo()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos.Count == 0){
                return RedirectToAction("Index");
            }

            var excelBytes = _excelInterface.ExportarProdutosParaExcel(produtos);
            var nomeDoArquivo = $"produtos.xlsx";

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeDoArquivo);
        }

    }
}
