using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSystem.API.DTOs;
using BankingSystem.API.Interfaces;
using BankingSystem.API.Models;

namespace BankingSystem.API.Services
{
    /// <summary>
    /// Handles business logic for creating and retrieving banking system users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDTO> CreateUserAsync(CreateUserDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("User name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("User email is required.");
            }

            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToResponseDTO(user);
        }

        public async Task<UserResponseDTO?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            return user is null ? null : MapToResponseDTO(user);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(MapToResponseDTO);
        }

        private static UserResponseDTO MapToResponseDTO(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
    }
}