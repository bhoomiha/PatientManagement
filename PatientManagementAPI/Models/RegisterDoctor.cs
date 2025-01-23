using System.ComponentModel.DataAnnotations;

namespace PatientManagementAPI.Models
{
    public class RegisterDoctor
    {   
        [Key]
        public int Id;
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
