﻿namespace Importa_planilha_excel.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public string Marca { get; set; }
    }
}
