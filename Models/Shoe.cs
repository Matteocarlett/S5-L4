using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Scarpe.Models
{
    public class Shoe
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public decimal Prezzo { get; set; }
        public string? Descrizione { get; set; }
        public string? Image { get; set; } = null;
        public string? Image1 { get; set; } = null;
        public string? Image2 { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;

    }
}
