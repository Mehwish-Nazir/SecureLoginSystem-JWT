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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using DocumentFormat.OpenXml.Wordprocessing;
using DataType = System.ComponentModel.DataAnnotations.DataType;
//using BackeEndAuthentication.SwaggerFilters;
namespace BackeEndAuthentication.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        //[Phone]
        [Required]
        [RegularExpression(@"^(\+92|0)?3\d{2}\d{7}$", ErrorMessage = "Invalid Pakistani phone number.")]
        [MaxLength(15)]

        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]

        public string Password { get; set; }  //temporary field to store password and store as PassowrdHash in actual model field

        [Required]
        [Compare("Password", ErrorMessage ="Password do not match, try again!")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ProfilePicture { get; set; }  // ❌ REMOVE [Required] if present!



        [BindNever]   //used [BindNever] to hide it from frontEnd and swagger UI but it's logic will work in service where there this property 'ProfileImagePath' is used
       //And write service to hide ProfileImagePath property from Swagger UI ad register in Program.cs file
        [JsonIgnore] 
       // [SwaggerExclude]    don't need this after applying filter of Schema and document filter in file 'ExcludeSwaggerPropertiesFilter'
       // public string ProfileImagePath { get; set; }
       internal string ProfileImagePath { get; set; }
        [BindNever]
        [JsonIgnore]  // or remove this if frontend will supply it
        internal int RoleId { get; set; } = 2; //default roleID=2 which is customer
    }

    public class RegisterResponseDTO
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
