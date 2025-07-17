using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BackeEndAuthentication.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BackeEndAuthentication.Models
{

    [Table("Users")]

    //Apply indexing on above of class for fast searching

    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]

    public class Users
    {
        //use this key annotation for int auto gnerated id
        //[Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid UserID { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        // [Phone]
        [Required]
        [RegularExpression(@"^(\+92|0)?3\d{2}\d{7}$", ErrorMessage = "Invalid Pakistani phone number.")]
        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]

        public string PasswordHash { get; set; }

    //    [MaxLength(255)]
//        public string PasswordSalt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginAttempts { get; set; } = 0; //keep track of invalid login attempts
        public DateTime? LockOutEnd { get; set; }

        [MaxLength(255)]
        public string? ProfileImagePath { get; set; }

        public string? EmailConfirmationToken { get; set; }

        public DateTime? EmailConfirmationExpiry { get; set; }
        [ForeignKey("Roles")]
        public int RoleId { get; set; }

        [JsonIgnore]
        //Each User → One Role
        public Role Role { get; set; }   // Navigation Property
    }
}
