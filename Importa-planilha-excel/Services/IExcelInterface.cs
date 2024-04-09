using Importa_planilha_excel.Models;

namespace Importa_planilha_excel.Services
{
    public interface IExcelInterface
    {
        MemoryStream LerStream(IFormFile formFile);
        List<ProdutoModel> LerXls(MemoryStream stream);
        void SalvarDados(List<ProdutoModel> produtos);
    }
}
