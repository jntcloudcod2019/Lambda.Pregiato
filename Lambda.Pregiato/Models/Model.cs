using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Lambda.Pregiato.Models
{
    public class Model
    {

        [Key]
        public Guid IdModel { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [JsonIgnore]

        [Required]
        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres.")]
        public string CPF { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "RG deve ter no máximo 20 caracteres.")]
        public string RG { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public int Age { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        public string NumberAddress { get; set; }

        [Required]
        public string Complement { get; set; }

        [StringLength(30)]
        public string BankAccount { get; set; }

        [Required]
        public bool Status { get; set; } = true;

        [Required]
        public string? Neighborhood { get; set; }
        [Required]
        public string? City { get; set; }

        [Required]
        public string UF { get; set; }

        [Required]
        public string TelefonePrincipal { get; set; }
        [Required]
        public string TelefoneSecundario { get; set; }

        [JsonIgnore]
        public JsonDocument? DNA { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public List<Contract> Contracts { get; set; } = new List<Contract>();

    }
}
