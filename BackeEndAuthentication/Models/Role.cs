using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace BackeEndAuthentication.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        [MaxLength]
        public string RoleName { get; set; }
        // Navigation: Each role can be assigned to many users
        
        [JsonIgnore] //  Prevents circular issues in API responses
        public  List<Users> Users { get; set; } = new List<Users>();
    }
}
