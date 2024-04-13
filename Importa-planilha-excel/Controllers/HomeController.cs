using Importa_planilha_excel.Data;
using Importa_planilha_excel.Models;
using Importa_planilha_excel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;



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
        [HttpGet]

        public IActionResult ExportarArquivo()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos.Count == 0){
                return RedirectToAction("Index");
            }
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Produtos");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Código";
            worksheet.Cell(1, 3).Value = "Nome";
            worksheet.Cell(1, 4).Value = "Valor";
            worksheet.Cell(1, 5).Value = "Quantidade";
            worksheet.Cell(1, 6).Value = "Marca";

            int row = 2;
            foreach (var produto in produtos)
            {
                worksheet.Cell(row, 1).Value = produto.Id;
                worksheet.Cell(row, 2).Value = produto.Codigo;
                worksheet.Cell(row, 3).Value = produto.Nome;
                worksheet.Cell(row, 4).Value = produto.Valor;
                worksheet.Cell(row, 5).Value = produto.Quantidade;
                worksheet.Cell(row, 6).Value = produto.Marca;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            var dataAtual = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var nomeDoArquivo = $"produtos-{dataAtual}.xlsx";

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                byte[] excelBytes = stream.ToArray();

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeDoArquivo);
            }
        }


    }
}
