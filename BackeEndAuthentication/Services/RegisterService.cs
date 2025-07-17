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
using Microsoft.AspNetCore.Mvc;
using BackeEndAuthentication.Repository;
using AutoMapper;
using BackeEndAuthentication.CustomExceptions;
using BackeEndAuthentication.Middleware;
using BackeEndAuthentication.Helpers;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Internal;
namespace BackeEndAuthentication.Services
{
    public class RegisterService :IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterService> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterService(IUserRepository userRepository, IMapper mapper, ILogger<RegisterService> logger, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
        }

        //RegisterResponseDTO is seperate DTO  for 'Response Body' & 
        //RegisterUserDTO is seperate DTO for 'Request Body'

        public async Task<RegisterResponseDTO> RegisterUserAsync(RegisterUserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email is null or empty.");
            }

            var userEmail = await _userRepository.GetUserByEmailAsync(dto.Email);
            bool userByEmailExists = userEmail != null;

            _logger.LogInformation("Looking for user by email: {Email}", dto.Email);

            if (userByEmailExists)
            {
                throw new ConflictException($"User with this email {dto.Email} already exist, use another one!");
            }

            var userPhone = await _userRepository.GetUserByPhoneNumber(dto.PhoneNumber);
            bool userByPhoneExists = userPhone != null;
            if (userByPhoneExists)
            {
                throw new ConflictException($"User with this phone number {dto.PhoneNumber} already exist, use another one!");
            }

            var userEntity = _mapper.Map<Users>(dto);

            // Handle profile image
            if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
            {
                var webRoot = Directory.GetCurrentDirectory();
                var folder = Path.Combine(webRoot, "wwwroot", "assets");
                Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ProfilePicture.FileName)}";
                var fullPath = Path.Combine(folder, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.ProfilePicture.CopyToAsync(stream);

                userEntity.ProfileImagePath = $"/assets/{fileName}";
            }
            else
            {
                userEntity.ProfileImagePath = "/assets/default_img.jpg";
            }

            //  Hash password using updated PasswordHelpers (BCrypt auto-generates salt)
            userEntity.PasswordHash = PasswordHelpers.HashPassword(dto.Password);

            //  Remove custom salt logic — not needed anymore
            //userEntity.PasswordSalt = null;

            userEntity.IsActive = true;
            userEntity.CreatedAt = DateTime.UtcNow;
            userEntity.IsEmailConfirmed = false;
            userEntity.FailedLoginAttempts = 0;

            //  Set default role
            userEntity.RoleId = await _userRepository.GetRoleIdByNameAsync("Customer");

            userEntity.EmailConfirmationToken = Guid.NewGuid().ToString();
            userEntity.EmailConfirmationExpiry = DateTime.UtcNow.AddHours(24);

            var context = _userRepository.GetDbContext();

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await _userRepository.AddAsync(userEntity);
                await _userRepository.SaveChangesAsync();

                _logger.LogInformation("User registered and transaction committed successfully for user: {User}", dto.Username);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed while registering user with email {Email}", dto.Email);
                await transaction.RollbackAsync();
                throw;
            }


            // Send confirmation email after committing transaction

            /*
           // var confirmationLink = $"https://yourfrontend.com/confirm-email?token={userEntity.EmailConfirmationToken}";
           // var emailBody = $"Hello {dto.Username},<br/>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.<br/>This link will expire in 24 hours.";
           // await _emailSender.SendEmailAsync(userEntity.Email, "Confirm your email", emailBody);
            */
            var responseDto = _mapper.Map<RegisterResponseDTO>(userEntity);
            return responseDto; 
        }

    }
}
