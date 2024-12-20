﻿    using DTOs;

    namespace BlazorApp.Services;

    public interface IUserService
    {
        public Task<UserDto> AddUserAsync(UserDto request);
        Task UpdateUserAsync(int id, UserDto request);
        Task<string> GetUserAsync(int id);
        Task<IEnumerable<UserDto>>GetAllUsers(string? username = null);
        Task DeleteUserAsync(int id);
        
        Task<UserDto> GetUserByUsername(string username);
        
    }