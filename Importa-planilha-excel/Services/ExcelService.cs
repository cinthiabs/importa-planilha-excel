using ClosedXML.Excel;
using Importa_planilha_excel.Data;
using Importa_planilha_excel.Models;
using OfficeOpenXml;
using ClosedXML.Excel;

namespace Importa_planilha_excel.Services
{
    public class ExcelService : IExcelInterface
    {
        private readonly AppDbContext _appDbContext;
        public ExcelService(AppDbContext context) 
        {
            _appDbContext = context;
        }


        public MemoryStream LerStream(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                formFile?.CopyTo(stream);
                var Listbyte = stream.ToArray();
                return new MemoryStream(Listbyte);
            }
        }

        public List<ProdutoModel> LerXls(MemoryStream stream)
        {
            try
            {
                var response = new List<ProdutoModel>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int numeroDeLinhas = worksheet.Dimension.End.Row;
                    for(int linha = 2; linha <= numeroDeLinhas; linha++)
                    {
                        if(worksheet.Cells[linha, 1].Value !=null && worksheet.Cells[linha, 2].Value != null)
                        {
                            var produto = new ProdutoModel();
                            produto.Id = 0;
                            produto.Codigo = worksheet.Cells[linha, 1].Value.ToString();
                            produto.Nome = worksheet.Cells[linha, 2].Value.ToString();
                            produto.Valor = Convert.ToDecimal(worksheet.Cells[linha, 3].Value);
                            produto.Quantidade = Convert.ToInt32(worksheet.Cells[linha, 4].Value);
                            produto.Marca = worksheet.Cells[linha, 5].Value.ToString();

                            response.Add(produto);
                        }
                    }
                       
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SalvarDados(List<ProdutoModel> produtos)
        {
            try
            {
                foreach (var produto in produtos)
                {
                    var existe = _appDbContext.Produtos.FirstOrDefault(p => p.Codigo == produto.Codigo);

                    if (existe == null)
                    {
                        _appDbContext.Add(produto);
                        
                    }
                }
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public byte[] ExportarProdutosParaExcel(List<ProdutoModel> produtos)
        {
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

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}
