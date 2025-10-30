using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBarbearia.Models
{
    public class Servico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }
    }
}
