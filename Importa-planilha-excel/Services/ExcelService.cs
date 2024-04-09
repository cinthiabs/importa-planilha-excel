using Importa_planilha_excel.Data;
using Importa_planilha_excel.Models;
using OfficeOpenXml;

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
                        if(worksheet.Cells[linha, 1].Value !=null && worksheet.Cells[linha, 4].Value != null)
                        {
                            var produto = new ProdutoModel();
                            produto.Id = 0;
                            produto.Nome = worksheet.Cells[linha, 1].Value.ToString();
                            produto.Valor = Convert.ToDecimal(worksheet.Cells[linha, 2].Value);
                            produto.Quantidade = Convert.ToInt32(worksheet.Cells[linha, 3].Value);
                            produto.Marca = worksheet.Cells[linha, 4].Value.ToString();

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
                foreach(var produto in produtos)
                {
                    _appDbContext.Add(produto);
                    _appDbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
