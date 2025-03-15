using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lambda.Pregiato.Models
{
    [Table("Contracts")]
    public class Contract
    {
       
        [Key]
        public Guid ContractId { get; set; }
        public string ContractFilePath { get; set; } = string.Empty;
        public byte[] Content { get; set; }
        public int CodProposta { get; set; }
    }
}
