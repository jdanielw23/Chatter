using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Chatter.Model;
using Chatter.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using User = Chatter.Model.Entities.User;

namespace Chatter.Services
{
    public class UserService : IUserService
    {
        private readonly DbContext _dbContext;
        private readonly IAuthService _authService;

        public UserService(DbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<string> RegisterUser(RegisterModel registerModel)
        {
            if (string.IsNullOrWhiteSpace(registerModel.Email) ||
                string.IsNullOrWhiteSpace(registerModel.Username) ||
                string.IsNullOrWhiteSpace(registerModel.Password))
                return null;

            var existsAlready = await _dbContext.Set<User>().AnyAsync(u => u.Email == registerModel.Email || u.Username == registerModel.Username);
            if (existsAlready)
                return null;

            var hmac = new HMACSHA512();
            var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password));

            var newUser = new User()
            {
                Email = registerModel.Email,
                Username = registerModel.Username,
                Password = hashedPassword,
                Salt = hmac.Key,
            };
            await _dbContext.Set<User>().AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return _authService.GenerateToken(new Claim(ClaimTypes.Name, $"{newUser.Id}"));
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Username) ||
                string.IsNullOrWhiteSpace(loginModel.Password))
                throw new ArgumentException("Login model is invalid");

            var user = await _dbContext.Set<User>().SingleAsync(u => u.Username == loginModel.Username);
            if (user == null)
                throw new Exception("User not found");

            if (!ValidatePassword(loginModel.Password, user.Password, user.Salt))
                return null;

            return _authService.GenerateToken(new Claim(ClaimTypes.Name, $"{user.Id}"));
        }

        public async Task<IEnumerable<User>> GetFriends(int userId)
        {
            var friends = new List<User>();
            var user = await _dbContext.Set<User>().FindAsync(userId);
            await _dbContext.Entry(user).Collection(u => u.FriendsTo).LoadAsync();
            foreach (var friendship in user.FriendsTo)
            {
                await _dbContext.Entry(friendship).Reference(f => f.User2).LoadAsync();
                friends.Add(friendship.User2);
            }

            return friends;
        }

        public async Task AddFriend(int userId, string username)
        {
            var loggedInUser = await _dbContext.Set<User>().FindAsync(userId);
            await _dbContext.Entry(loggedInUser).Collection(u => u.FriendsFrom).LoadAsync();

            var friend = await _dbContext.Set<User>().SingleAsync(u => u.Username == username);
            if (loggedInUser.FriendsFrom.Any(u => u.UserId1 == friend.Id || u.UserId2 == friend.Id))
                throw new Exception("Already friends");

            loggedInUser.FriendsFrom.Add(new Friendship()
            {
                User2 = friend
            });

            await _dbContext.SaveChangesAsync();
        }

        private bool ValidatePassword(string submittedPassword, byte[] password, byte[] salt)
        {
            var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(submittedPassword));
            if (computedHash.Length != password.Length)
                return true;

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (password[i] != computedHash[i])
                    return false;
            }

            return true;
        }
    }

    public interface IUserService
    {
        Task<string> RegisterUser(RegisterModel registerModel);
        Task<string> Login(LoginModel loginModel);
        Task AddFriend(int userId, string username);
        Task<IEnumerable<User>> GetFriends(int userId);
    }
}