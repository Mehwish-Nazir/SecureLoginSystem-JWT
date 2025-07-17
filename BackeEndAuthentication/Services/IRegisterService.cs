using BackeEndAuthentication.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BackeEndAuthentication.Models;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using BackeEndAuthentication.DTO;
using Azure.Identity;
//using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BackeEndAuthentication.Services
{
    public interface IRegisterService
    {
        Task<RegisterResponseDTO> RegisterUserAsync(RegisterUserDTO dto);
    }
}
