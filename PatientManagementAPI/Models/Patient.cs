using System.ComponentModel.DataAnnotations;
namespace PatientManagementAPI.Models
{
    public class Patient

    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Illness { get; set; }
        [Required]
        public DateTime AdmissionDate { get; set; }
        [Required]
        public DateTime RecheckUpDate { get; set; }
        public string AdditionalDetails { get; set; }
    }
}
