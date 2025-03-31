using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lambda.Pregiato.Enum;

namespace Lambda.Pregiato.Models
{
    [Table("Contracts")]
    public class Contract
    {

        [Key]
        public Guid ContractId { get; set; }
        public Guid? PaymentId { get; set; }
        public string? CodProducers { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime DataContrato { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime VigenciaContrato { get; set; } = DateTime.UtcNow;
        [Required]
        public decimal ValorContrato { get; set; }
        [Required]
        public string? FormaPagamento { get; set; }
        [Required]
        public string StatusPagamento { get; set; } = "Paid";
        [Required]
        public string ContractFilePath { get; set; } = string.Empty;
        [Required]
        public byte[]? Content { get; set; }
        [Required]
        public int CodProposta { get; set; }
        [Required]
        public StatusContratc StatusContratc { get; set; } = StatusContratc.Ativo;
        public Guid? IdModel { get; set; }
        [Required]
        public  string TemplateFileName { get; }

    }
}
