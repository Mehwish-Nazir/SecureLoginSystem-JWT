using System.Collections.Generic;
using BackeEndAuthentication.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;
namespace BackeEndAuthentication.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]

        public string Password { get; set; }  //temporary field to store password and store as PassowrdHash in actual model field
    }

    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        //don't have to return password for security purpose
       // public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

    }
}
